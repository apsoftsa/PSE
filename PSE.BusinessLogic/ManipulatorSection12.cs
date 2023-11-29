using System.Globalization;
using PSE.BusinessLogic.Calculations;
using PSE.BusinessLogic.Common;
using PSE.BusinessLogic.Interfaces;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using PSE.Model.SupportTables;
using PSE.Model.Params;
using static PSE.Model.Common.Constants;
using static PSE.Model.Common.Enumerations;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection12 : ManipulatorBase, IManipulator
    {
        
        private readonly BondsCalculation _calcBonds;

        public ManipulatorSection12(CalculationSettings calcSettings, CultureInfo? culture = null) : base(PositionClassifications.OBBLIGAZIONI_CON_SCADENZA_MINOR_OR_EQUAL_5_ANNI, ManipolationTypes.AsSection12, culture) 
        {            
            _calcBonds = new BondsCalculation(calcSettings);
        }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            SectionBinding _sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section12 _output = new()
            {
                SectionId = _sectionDest.SectionId,
                SectionCode = _sectionDest.SectionCode,
                SectionName = _sectionDest.SectionContent
            };
            if (extractedData.Any(_flt => _flt.RecordType == nameof(IDE)) && extractedData.Any(_flt => _flt.RecordType == nameof(POS)))
            {
                decimal _currencyRate, _customerSumAmounts, _quoteType, _currentBaseValue;
                IBondsMaturingLessThan5Years _bondLessThan5;
                ISection12Content _sectionContent;                
                List<IDE> _ideItems = extractedData.Where(_flt => _flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> _posItems = extractedData.Where(_flt => _flt.AlreadyUsed == false && _flt.RecordType == nameof(POS)).OfType<POS>().Where(_fltSubCat => ManipulatorOperatingRules.IsRowDestinatedToManipulator(this, _fltSubCat.SubCat4_15));
                IEnumerable<CUR> _curItems = extractedData.Where(_flt => _flt.RecordType == nameof(CUR)).OfType<CUR>(); 
                foreach (IDE _ideItem in _ideItems)
                {
                    _customerSumAmounts = extractedData.Where(_flt => _flt.RecordType == nameof(POS)).OfType<POS>().Where(_subFlt => _subFlt.CustomerNumber_2 == _ideItem.CustomerNumber_2 && _subFlt.Amount1Base_23.HasValue).Sum(_sum => _sum.Amount1Base_23.Value);
                    _customerSumAmounts += extractedData.Where(_flt => _flt.RecordType == nameof(POS)).OfType<POS>().Where(_subFlt => _subFlt.CustomerNumber_2 == _ideItem.CustomerNumber_2 && _subFlt.ProRataBase_56.HasValue).Sum(_sum => _sum.ProRataBase_56.Value);
                    if (_posItems != null && _posItems.Any(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2))
                    {
                        _sectionContent = new Section12Content();                        
                        foreach (POS _posItem in _posItems.Where(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2))                        
                        {
                            _currentBaseValue = _posItem.Amount1Base_23.HasValue ? _posItem.Amount1Base_23.Value : 0;
                            _currentBaseValue += _posItem.ProRataBase_56.HasValue ? _posItem.ProRataBase_56.Value : 0;
                            _quoteType = string.IsNullOrEmpty(_posItem.QuoteType_51) == false && _posItem.QuoteType_51.Trim() == "%" ? 100m : 1m;
                            _bondLessThan5 = new BondsMaturingLessThan5Years()
                            {
                                ValorNumber = _posItem.NumSecurity_29 != null ? _posItem.NumSecurity_29 : 0,
                                Currency = _posItem.Currency1_17,
                                Description = ((string.IsNullOrEmpty(_posItem.Description1_32) ? "" : _posItem.Description1_32) + " " + (string.IsNullOrEmpty(_posItem.Description2_33) ? "" : _posItem.Description2_33)).Trim(),
                                Expiration = _posItem.MaturityDate_36 != null ? ((DateTime)_posItem.MaturityDate_36).ToString(DEFAULT_DATE_FORMAT, _culture) : "",
                                CurrentPrice = _posItem.Quote_48 != null ? _posItem.Quote_48.Value : 0,
                                PurchasePrice = _posItem.BuyPriceHistoric_53 != null ? _posItem.BuyPriceHistoric_53.Value : 0,
                                Isin = _posItem.IsinIban_85,
                                PriceBeginningYear = _posItem.BuyPriceAverage_87 != null ? _posItem.BuyPriceAverage_87.Value : 0,
                                PercentCoupon = _posItem.InterestRate_47 != null ? _posItem.InterestRate_47.Value : 0,
                                PercentYTM = 0, // not still recovered (!)
                                NominalAmount = _posItem.Quantity_28 != null ? _posItem.Quantity_28.Value : 0,
                                ExchangeRateImpactPurchase = _posItem.BuyExchangeRateHistoric_66 != null ? _posItem.BuyExchangeRateHistoric_66.Value : 0, //temporary
                                SPRating = (string.IsNullOrEmpty(_posItem.AgeRat_97) == false && _posItem.AgeRat_97.Trim() == "SP") ? _posItem.Rating_98 : string.Empty,
                                MsciEsg = (string.IsNullOrEmpty(_posItem.AgeRat_97) == false && _posItem.AgeRat_97.Trim() == "ES") ? _posItem.Rating_98 : string.Empty,
                                ExchangeRateImpactYTD = _posItem.BuyExchangeRateAverage_88 != null ? _posItem.BuyExchangeRateAverage_88.Value : 0, // temporary                                                             
                                PercentAsset = _customerSumAmounts != 0 && _currentBaseValue != 0 ? Math.Round(_currentBaseValue / _customerSumAmounts * 100m, _calcBonds.MeaningfulDecimalDigits) : 0
                            };
                            _currencyRate = (_curItems != null && _curItems.Any(_flt => _flt.CustomerNumber_2 == _posItem.CustomerNumber_2 && _flt.Currency_5 == _bondLessThan5.Currency && _flt.Rate_6 != null)) ? _curItems.First(_flt => _flt.CustomerNumber_2 == _posItem.CustomerNumber_2 && _flt.Currency_5 == _bondLessThan5.Currency && _flt.Rate_6 != null).Rate_6.Value : 0;
                            _bondLessThan5.PerformancePurchase = Math.Round((_bondLessThan5.CurrentPrice.Value - _bondLessThan5.PurchasePrice.Value) * _bondLessThan5.NominalAmount.Value / _quoteType, _calcBonds.MeaningfulDecimalDigits);
                            _bondLessThan5.PerformanceYTD = Math.Round((_bondLessThan5.CurrentPrice.Value - _bondLessThan5.PriceBeginningYear.Value) * _bondLessThan5.NominalAmount.Value / _quoteType, _calcBonds.MeaningfulDecimalDigits);
                            _bondLessThan5.PercentPerformancePurchase = _calcBonds.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcBonds.GetSign(_bondLessThan5.NominalAmount, _bondLessThan5.CurrentPrice), _bondLessThan5.PurchasePrice.Value, _bondLessThan5.CurrentPrice.Value));
                            _bondLessThan5.PercentPerformanceYTD = _calcBonds.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcBonds.GetSign(_bondLessThan5.NominalAmount, _bondLessThan5.CurrentPrice), _bondLessThan5.PriceBeginningYear.Value, _bondLessThan5.CurrentPrice.Value));
                            _bondLessThan5.ExchangeRateImpactPurchase = _calcBonds.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcBonds.GetSign(_bondLessThan5.NominalAmount, _bondLessThan5.CurrentPrice), _bondLessThan5.ExchangeRateImpactPurchase.Value, _currencyRate));
                            _bondLessThan5.ExchangeRateImpactYTD = _calcBonds.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcBonds.GetSign(_bondLessThan5.NominalAmount, _bondLessThan5.CurrentPrice), _bondLessThan5.ExchangeRateImpactYTD.Value, _currencyRate));
                            _sectionContent.BondsMaturingLessThan5Years.Add(_bondLessThan5);
                            _posItem.AlreadyUsed = true;
                        }
                        _output.Content = new Section12Content(_sectionContent);
                    }
                }
            }
            return _output;
        }

    }

}
