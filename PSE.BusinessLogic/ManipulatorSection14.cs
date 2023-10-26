using System.Globalization;
using PSE.BusinessLogic.Calculations;
using PSE.BusinessLogic.Common;
using PSE.BusinessLogic.Interfaces;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using PSE.Model.Params;
using PSE.Model.SupportTables;
using static PSE.Model.Common.Constants;
using static PSE.Model.Common.Enumerations;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection14 : ManipulatorBase, IManipulator
    {
        
        private readonly BondsCalculation _calcBonds;

        public ManipulatorSection14(CalculationSettings calcSettings, CultureInfo? culture = null) : base(PositionClassifications.OBBLIGAZIONI_CON_SCADENZA_MAJOR_THAN_5_ANNI_FONDI_OBBLIGAZIONARI, ManipolationTypes.AsSection14, culture) 
        {
            _calcBonds = new BondsCalculation(calcSettings);            
        }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            SectionBinding _sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section14 _output = new()
            {
                SectionId = _sectionDest.SectionId,
                SectionCode = _sectionDest.SectionCode,
                SectionName = _sectionDest.SectionContent
            };
            if (extractedData.Any(_flt => _flt.RecordType == nameof(IDE)) && extractedData.Any(_flt => _flt.RecordType == nameof(POS)))
            {
                decimal _currencyRate, _customerSumAmounts, _currentBaseValue, _quoteType;
                IObligationsWithMaturityGreatherThanFiveYears _oblWithMatGreatThan5;
                ISection14Content _sectionContent;
                List<IDE> _ideItems = extractedData.Where(_flt => _flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> _posItems = extractedData.Where(_flt => _flt.AlreadyUsed == false && _flt.RecordType == nameof(POS)).OfType<POS>().Where(_fltSubCat => ManipulatorOperatingRules.IsRowDestinatedToManipulator(this, _fltSubCat.SubCat4_15));
                IEnumerable<CUR> _curItems = extractedData.Where(_flt => _flt.RecordType == nameof(CUR)).OfType<CUR>();
                foreach (IDE _ideItem in _ideItems)
                {
                    _customerSumAmounts = extractedData.Where(_flt => _flt.RecordType == nameof(POS)).OfType<POS>().Where(_subFlt => _subFlt.CustomerNumber_2 == _ideItem.CustomerNumber_2 && _subFlt.Amount1Base_23.HasValue).Sum(_sum => _sum.Amount1Base_23.Value);
                    _customerSumAmounts += extractedData.Where(_flt => _flt.RecordType == nameof(POS)).OfType<POS>().Where(_subFlt => _subFlt.CustomerNumber_2 == _ideItem.CustomerNumber_2 && _subFlt.ProRataBase_56.HasValue).Sum(_sum => _sum.ProRataBase_56.Value);
                    if (_posItems != null && _posItems.Any(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2))
                    {
                        _sectionContent = new Section14Content();
                        foreach (POS _posItem in _posItems.Where(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2))
                        {
                            _currentBaseValue = _posItem.Amount1Base_23.HasValue ? _posItem.Amount1Base_23.Value : 0;
                            _currentBaseValue += _posItem.ProRataBase_56.HasValue ? _posItem.ProRataBase_56.Value : 0;
                            _quoteType = string.IsNullOrEmpty(_posItem.QuoteType_51) == false && _posItem.QuoteType_51.Trim() == "%" ? 100m : 1m;
                            _oblWithMatGreatThan5 = new ObligationsWithMaturityGreatherThanFiveYears()
                            {
                                ValorNumber = _posItem.NumSecurity_29 != null ? _posItem.NumSecurity_29 : 0,
                                Currency = _posItem.Currency1_17,
                                Description = ((string.IsNullOrEmpty(_posItem.Description1_32) ? "" : _posItem.Description1_32) + " " + (string.IsNullOrEmpty(_posItem.Description2_33) ? "" : _posItem.Description2_33)).Trim(),
                                CurrentPrice = _posItem.Quote_48 != null ? _posItem.Quote_48.Value : 0,
                                PurchasePrice = _posItem.BuyPriceHistoric_53 != null ? _posItem.BuyPriceHistoric_53.Value : 0,
                                PriceBeginningYear = _posItem.BuyPriceAverage_87 != null ? _posItem.BuyPriceAverage_87.Value : 0,
                                Expiration = _posItem.MaturityDate_36 != null ? ((DateTime)_posItem.MaturityDate_36).ToString(DEFAULT_DATE_FORMAT, _culture) : "",
                                Isin = _posItem.IsinIban_85,
                                PercentCoupon = _posItem.InterestRate_47 != null ? _posItem.InterestRate_47.Value : 0,
                                PercentYTM = 0, // not still recovered (!)
                                NominalAmount = _posItem.Quantity_28 != null ? _posItem.Quantity_28.Value : 0,
                                SPRating = (string.IsNullOrEmpty(_posItem.AgeRat_97) == false && _posItem.AgeRat_97.Trim() == "SP") ? _posItem.Rating_98 : string.Empty,
                                MsciEsg = (string.IsNullOrEmpty(_posItem.AgeRat_97) == false && _posItem.AgeRat_97.Trim() == "ES") ? _posItem.Rating_98 : string.Empty,
                                //ExchangeRateImpactPurchase = _posItem.BuyExchangeRateHistoric_66 != null ? _posItem.BuyExchangeRateHistoric_66.Value : 0, //temporary                               
                                //ExchangeRateImpactYTD = _posItem.BuyExchangeRateAverage_88 != null ? _posItem.BuyExchangeRateAverage_88.Value : 0, //temporary
                                PercentAsset = _customerSumAmounts != 0 && _currentBaseValue != 0 ? Math.Round(_currentBaseValue / _customerSumAmounts * 100m, _calcBonds.MeaningfulDecimalDigits) : 0
                            };
                            _currencyRate = (_curItems != null && _curItems.Any(_flt => _flt.CustomerNumber_2 == _posItem.CustomerNumber_2 && _flt.Currency_5 == _oblWithMatGreatThan5.Currency && _flt.Rate_6 != null)) ? _curItems.First(_flt => _flt.CustomerNumber_2 == _posItem.CustomerNumber_2 && _flt.Currency_5 == _oblWithMatGreatThan5.Currency && _flt.Rate_6 != null).Rate_6.Value : 0;
                            _oblWithMatGreatThan5.PerformancePurchase = Math.Round((_oblWithMatGreatThan5.CurrentPrice.Value - _oblWithMatGreatThan5.PurchasePrice.Value) * _oblWithMatGreatThan5.NominalAmount.Value / _quoteType, _calcBonds.MeaningfulDecimalDigits);
                            _oblWithMatGreatThan5.PerformanceYTD = Math.Round((_oblWithMatGreatThan5.CurrentPrice.Value - _oblWithMatGreatThan5.PriceBeginningYear.Value) * _oblWithMatGreatThan5.NominalAmount.Value / _quoteType, _calcBonds.MeaningfulDecimalDigits);
                            _oblWithMatGreatThan5.PercentPerformancePurchase = _calcBonds.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcBonds.GetSign(_oblWithMatGreatThan5.NominalAmount, _oblWithMatGreatThan5.CurrentPrice), _oblWithMatGreatThan5.CurrentPrice.Value, _oblWithMatGreatThan5.PurchasePrice.Value));
                            _oblWithMatGreatThan5.PercentPerformanceYTD = _calcBonds.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcBonds.GetSign(_oblWithMatGreatThan5.NominalAmount, _oblWithMatGreatThan5.CurrentPrice), _oblWithMatGreatThan5.CurrentPrice.Value, _oblWithMatGreatThan5.PriceBeginningYear.Value));
                            //_oblWithMatGreatThan5.ExchangeRateImpactPurchase = _calcBonds.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcBonds.GetSign(_oblWithMatGreatThan5.NominalAmount, _currencyRate), _oblWithMatGreatThan5.CurrentPrice.Value, _oblWithMatGreatThan5.ExchangeRateImpactPurchase.Value));
                            //_oblWithMatGreatThan5.ExchangeRateImpactYTD = _calcBonds.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcBonds.GetSign(_oblWithMatGreatThan5.NominalAmount, _currencyRate), _oblWithMatGreatThan5.CurrentPrice.Value, _oblWithMatGreatThan5.ExchangeRateImpactYTD.Value));
                            _oblWithMatGreatThan5.ExchangeRateImpactPurchase = Math.Round(_oblWithMatGreatThan5.NominalAmount.Value * _oblWithMatGreatThan5.CurrentPrice.Value / 100m, _calcBonds.MeaningfulDecimalDigits);
                            _oblWithMatGreatThan5.ExchangeRateImpactYTD = Math.Round(_oblWithMatGreatThan5.NominalAmount.Value * _oblWithMatGreatThan5.PriceBeginningYear.Value / 100m, _calcBonds.MeaningfulDecimalDigits);
                            _sectionContent.ObligationsWithMaturityGreatherThanFiveYears.Add(_oblWithMatGreatThan5);
                            _posItem.AlreadyUsed = true;
                        }
                        _output.Content = new Section14Content(_sectionContent);
                    }
                }
            }
            return _output;
        }

    }

}
