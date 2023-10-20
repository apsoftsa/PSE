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

    public class ManipulatorSection13 : ManipulatorBase, IManipulator
    {
        
        private readonly BondsCalculation _calcBonds;

        public ManipulatorSection13(CalculationSettings calcSettings, CultureInfo? culture = null) : base(PositionClassifications.OBBLIGAZIONI_CON_SCADENZA_MINOR_OR_EQUAL_1_ANNO, ManipolationTypes.AsSection13, culture) 
        {
            _calcBonds = new BondsCalculation(calcSettings);
        }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            SectionBinding _sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section13 _output = new()
            {
                SectionId = _sectionDest.SectionId,
                SectionCode = _sectionDest.SectionCode,
                SectionName = _sectionDest.SectionContent
            };
            if (extractedData.Any(_flt => _flt.RecordType == nameof(IDE)) && extractedData.Any(_flt => _flt.RecordType == nameof(POS)))
            {
                decimal _currencyRate;
                IBondsMinorOrEqualTo1Year _bondMinOrEquTo1Year;
                ISection13Content _sectionContent;
                List<IDE> _ideItems = extractedData.Where(_flt => _flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> _posItems = extractedData.Where(_flt => _flt.AlreadyUsed == false && _flt.RecordType == nameof(POS)).OfType<POS>().Where(_fltSubCat => ManipulatorOperatingRules.IsRowDestinatedToManipulator(this, _fltSubCat.SubCat4_15));
                IEnumerable<CUR> _curItems = extractedData.Where(_flt => _flt.RecordType == nameof(CUR)).OfType<CUR>();
                foreach (IDE _ideItem in _ideItems)
                {
                    if (_posItems != null && _posItems.Any(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2))
                    {
                        _sectionContent = new Section13Content();
                        foreach (POS _posItem in _posItems)
                        {
                            _bondMinOrEquTo1Year = new BondsMinorOrEqualTo1Year()
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
                                ExchangeRateImpactYTD = _posItem.BuyExchangeRateAverage_88 != null ? _posItem.BuyExchangeRateAverage_88.Value : 0, //temporary
                                PercentAsset = 0 // not still recovered (!)
                            };
                            _currencyRate = (_curItems != null && _curItems.Any(_flt => _flt.CustomerNumber_2 == _posItem.CustomerNumber_2 && _flt.Currency_5 == _bondMinOrEquTo1Year.Currency && _flt.Rate_6 != null)) ? _curItems.First(_flt => _flt.CustomerNumber_2 == _posItem.CustomerNumber_2 && _flt.Currency_5 == _bondMinOrEquTo1Year.Currency && _flt.Rate_6 != null).Rate_6.Value : 0;
                            _bondMinOrEquTo1Year.PerformancePurchase = Math.Round((_bondMinOrEquTo1Year.CurrentPrice.Value - _bondMinOrEquTo1Year.PurchasePrice.Value) * _bondMinOrEquTo1Year.NominalAmount.Value, _calcBonds.MeaningfulDecimalDigits);
                            _bondMinOrEquTo1Year.PerformanceYTD = Math.Round((_bondMinOrEquTo1Year.CurrentPrice.Value - _bondMinOrEquTo1Year.PriceBeginningYear.Value) * _bondMinOrEquTo1Year.NominalAmount.Value, _calcBonds.MeaningfulDecimalDigits); 
                            _bondMinOrEquTo1Year.PercentPerformancePurchase = _calcBonds.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcBonds.GetSign(_bondMinOrEquTo1Year.NominalAmount, _bondMinOrEquTo1Year.CurrentPrice), _bondMinOrEquTo1Year.CurrentPrice.Value, _bondMinOrEquTo1Year.PurchasePrice.Value));
                            _bondMinOrEquTo1Year.PercentPerformanceYTD = _calcBonds.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcBonds.GetSign(_bondMinOrEquTo1Year.NominalAmount, _bondMinOrEquTo1Year.CurrentPrice), _bondMinOrEquTo1Year.CurrentPrice.Value, _bondMinOrEquTo1Year.PriceBeginningYear.Value));
                            _bondMinOrEquTo1Year.ExchangeRateImpactPurchase = _calcBonds.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcBonds.GetSign(_bondMinOrEquTo1Year.NominalAmount, _currencyRate), _bondMinOrEquTo1Year.CurrentPrice.Value, _bondMinOrEquTo1Year.ExchangeRateImpactPurchase.Value));
                            _bondMinOrEquTo1Year.ExchangeRateImpactYTD = _calcBonds.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcBonds.GetSign(_bondMinOrEquTo1Year.NominalAmount, _currencyRate), _bondMinOrEquTo1Year.CurrentPrice.Value, _bondMinOrEquTo1Year.ExchangeRateImpactYTD.Value));
                            _sectionContent.BondsMaturingMinorOrEqualTo1Year.Add(_bondMinOrEquTo1Year);
                            _posItem.AlreadyUsed = true;
                        }
                        _output.Content = new Section13Content(_sectionContent);
                    }
                }
            }
            return _output;
        }

    }

}
