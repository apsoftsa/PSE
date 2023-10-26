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
using static PSE.Model.Common.Enumerations;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection16And17 : ManipulatorBase, IManipulator
    {

        private readonly FundsCalculation _calcFunds;

        public ManipulatorSection16And17(CalculationSettings calcSettings, CultureInfo? culture = null) : base(new List<PositionClassifications>()
            {
                PositionClassifications.OBBLIGAZIONI_CON_SCADENZA_MINOR_OR_EQUAL_1_ANNO,
                PositionClassifications.OBBLIGAZIONI_CON_SCADENZA_MINOR_OR_EQUAL_5_ANNI,
                PositionClassifications.OBBLIGAZIONI_CON_SCADENZA_MAJOR_THAN_5_ANNI_FONDI_OBBLIGAZIONARI,
                PositionClassifications.AZIONI_FONDI_AZIONARI,
                PositionClassifications.CONTI_METALLO_METALLI_FONDI_METALLO,
                PositionClassifications.FONDI_MISTI,
                PositionClassifications.PARTECIPAZIONI_IMMOBILIARI_FONDI_IMMOBILIARI
            }
            , ManipolationTypes.AsSection16And17, culture)
        {
            _calcFunds = new FundsCalculation(calcSettings);
        }

        public override string GetObjectNameDestination(IInputRecord inputRecord)
        {
            string _destinationObjectName = String.Empty;
            if (inputRecord != null && inputRecord.GetType() == typeof(POS))
            {
                string _subCategory = ((POS)inputRecord).SubCat4_15;
                string _category = ((POS)inputRecord).Category_11;
                if (_subCategory != string.Empty && _category != string.Empty && _category.Trim().Length >= 8 
                    && Enum.IsDefined(typeof(PositionClassifications), int.Parse(_subCategory)))
                {
                    PositionClassifications _currPosClass = (PositionClassifications)int.Parse(_subCategory);
                    if (this.PositionClassificationsSource.Contains((PositionClassifications)int.Parse(_subCategory)))
                    {
                        switch (_category.Substring(6,2).ToUpper())
                        {
                            case "FO":
                                {
                                    if(_currPosClass == PositionClassifications.OBBLIGAZIONI_CON_SCADENZA_MINOR_OR_EQUAL_1_ANNO
                                        || _currPosClass == PositionClassifications.OBBLIGAZIONI_CON_SCADENZA_MINOR_OR_EQUAL_5_ANNI
                                        || _currPosClass == PositionClassifications.OBBLIGAZIONI_CON_SCADENZA_MAJOR_THAN_5_ANNI_FONDI_OBBLIGAZIONARI)
                                        _destinationObjectName = "BondFunds";
                                    break;
                                }
                            case "FA":
                                {
                                    if (_currPosClass == PositionClassifications.AZIONI_FONDI_AZIONARI)
                                        _destinationObjectName = "EquityFunds";
                                    break;
                                }
                            case "FI":
                                {
                                    if (_currPosClass == PositionClassifications.PARTECIPAZIONI_IMMOBILIARI_FONDI_IMMOBILIARI)
                                        _destinationObjectName = "RealEstateFunds";
                                    break;
                                }
                            case "MF":
                                {
                                    if (_currPosClass == PositionClassifications.CONTI_METALLO_METALLI_FONDI_METALLO)
                                        _destinationObjectName = "MetalFunds";
                                    break;
                                }
                            case "FM":
                                {
                                    if (_currPosClass == PositionClassifications.FONDI_MISTI)
                                        _destinationObjectName = "MixedFunds";
                                    break;
                                }
                        }
                    }
                }
            }
            return _destinationObjectName;
        }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            SectionBinding _sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section16And17 _output = new()
            {
                SectionId = _sectionDest.SectionId,
                SectionCode = _sectionDest.SectionCode,
                SectionName = _sectionDest.SectionContent
            };
            if (extractedData.Any(_flt => _flt.RecordType == nameof(IDE)) && extractedData.Any(_flt => _flt.RecordType == nameof(POS)))
            {
                decimal _currencyRate, _customerSumAmounts, _currentBaseValue, _quoteType;
                string _destinationObjectName;
                IFundDetails _fundDetails;
                ISection16And17Content _sectionContent;
                List<IDE> _ideItems = extractedData.Where(_flt => _flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> _posItems = extractedData.Where(_flt => _flt.AlreadyUsed == false && _flt.RecordType == nameof(POS)).OfType<POS>().Where(_fltSubCat => ManipulatorOperatingRules.IsRowDestinatedToManipulator(this, _fltSubCat.SubCat4_15));
                IEnumerable<CUR> _curItems = extractedData.Where(_flt => _flt.RecordType == nameof(CUR)).OfType<CUR>();
                foreach (IDE _ideItem in _ideItems)
                {
                    _customerSumAmounts = extractedData.Where(_flt => _flt.RecordType == nameof(POS)).OfType<POS>().Where(_subFlt => _subFlt.CustomerNumber_2 == _ideItem.CustomerNumber_2 && _subFlt.Amount1Base_23.HasValue).Sum(_sum => _sum.Amount1Base_23.Value);
                    _customerSumAmounts += extractedData.Where(_flt => _flt.RecordType == nameof(POS)).OfType<POS>().Where(_subFlt => _subFlt.CustomerNumber_2 == _ideItem.CustomerNumber_2 && _subFlt.ProRataBase_56.HasValue).Sum(_sum => _sum.ProRataBase_56.Value);
                    if (_posItems != null && _posItems.Any(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2))
                    {
                        _sectionContent = new Section16And17Content();
                        foreach (POS _posItem in _posItems.Where(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2))
                        {
                            if ((_destinationObjectName = GetObjectNameDestination(_posItem)) != string.Empty)
                            {
                                _currentBaseValue = _posItem.Amount1Base_23.HasValue ? _posItem.Amount1Base_23.Value : 0;
                                _currentBaseValue += _posItem.ProRataBase_56.HasValue ? _posItem.ProRataBase_56.Value : 0;
                                _quoteType = string.IsNullOrEmpty(_posItem.QuoteType_51) == false && _posItem.QuoteType_51.Trim() == "%" ? 100m : 1m;
                                _fundDetails = new FundDetail()
                                {
                                    ValorNumber = _posItem.NumSecurity_29 != null ? _posItem.NumSecurity_29 : 0,
                                    Currency = _posItem.Currency1_17,
                                    Description = ((string.IsNullOrEmpty(_posItem.Description1_32) ? "" : _posItem.Description1_32) + " " + (string.IsNullOrEmpty(_posItem.Description2_33) ? "" : _posItem.Description2_33)).Trim(),
                                    CurrentPrice = _posItem.Quote_48 != null ? _posItem.Quote_48.Value : 0,
                                    PurchasePrice = _posItem.BuyPriceHistoric_53 != null ? _posItem.BuyPriceHistoric_53.Value : 0,
                                    PriceBeginningYear = _posItem.BuyPriceAverage_87 != null ? _posItem.BuyPriceAverage_87.Value : 0,
                                    NominalAmount = _posItem.Quantity_28 != null ? _posItem.Quantity_28.Value : 0,
                                    //ExchangeRateImpactPurchase = _posItem.BuyExchangeRateHistoric_66 != null ? _posItem.BuyExchangeRateHistoric_66.Value : 0, //temporary
                                    Isin = _posItem.IsinIban_85,
                                    SPRating = (string.IsNullOrEmpty(_posItem.AgeRat_97) == false && _posItem.AgeRat_97.Trim() == "SP") ? _posItem.Rating_98 : string.Empty,
                                    MsciEsg = (string.IsNullOrEmpty(_posItem.AgeRat_97) == false && _posItem.AgeRat_97.Trim() == "ES") ? _posItem.Rating_98 : string.Empty,
                                    //ExchangeRateImpactYTD = _posItem.BuyExchangeRateAverage_88 != null ? _posItem.BuyExchangeRateAverage_88.Value : 0, //temporary
                                    PercentAsset = _customerSumAmounts != 0 && _currentBaseValue != 0 ? Math.Round(_currentBaseValue / _customerSumAmounts * 100m, _calcFunds.MeaningfulDecimalDigits) : 0
                                };
                                _currencyRate = (_curItems != null && _curItems.Any(_flt => _flt.CustomerNumber_2 == _posItem.CustomerNumber_2 && _flt.Currency_5 == _fundDetails.Currency && _flt.Rate_6 != null)) ? _curItems.First(_flt => _flt.CustomerNumber_2 == _posItem.CustomerNumber_2 && _flt.Currency_5 == _fundDetails.Currency && _flt.Rate_6 != null).Rate_6.Value : 0;
                                _fundDetails.PerformancePurchase = Math.Round((_fundDetails.CurrentPrice.Value - _fundDetails.PurchasePrice.Value) * _fundDetails.NominalAmount.Value / _quoteType, _calcFunds.MeaningfulDecimalDigits);
                                _fundDetails.PerformanceYTD = Math.Round((_fundDetails.CurrentPrice.Value - _fundDetails.PriceBeginningYear.Value) * _fundDetails.NominalAmount.Value / _quoteType, _calcFunds.MeaningfulDecimalDigits);
                                _fundDetails.PercentPerformancePurchase = _calcFunds.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcFunds.GetSign(_fundDetails.NominalAmount, _fundDetails.CurrentPrice), _fundDetails.CurrentPrice.Value, _fundDetails.PurchasePrice.Value));
                                _fundDetails.PercentPerformanceYTD = _calcFunds.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcFunds.GetSign(_fundDetails.NominalAmount, _fundDetails.CurrentPrice), _fundDetails.CurrentPrice.Value, _fundDetails.PriceBeginningYear.Value));
                                //_fundDetails.ExchangeRateImpactPurchase = _calcFunds.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcFunds.GetSign(_fundDetails.NominalAmount, _currencyRate), _fundDetails.CurrentPrice.Value, _fundDetails.ExchangeRateImpactPurchase.Value));
                                //_fundDetails.ExchangeRateImpactYTD = _calcFunds.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcFunds.GetSign(_fundDetails.NominalAmount, _currencyRate), _fundDetails.CurrentPrice.Value, _fundDetails.ExchangeRateImpactYTD.Value));
                                _fundDetails.ExchangeRateImpactPurchase = Math.Round(_fundDetails.NominalAmount.Value * _fundDetails.CurrentPrice.Value / 100m, _calcFunds.MeaningfulDecimalDigits);
                                _fundDetails.ExchangeRateImpactYTD = Math.Round(_fundDetails.NominalAmount.Value * _fundDetails.PriceBeginningYear.Value / 100m, _calcFunds.MeaningfulDecimalDigits);
                                if (_destinationObjectName == "BondFunds")
                                    _sectionContent.BondFunds.Add(_fundDetails);
                                else if (_destinationObjectName == "EquityFunds")
                                    _sectionContent.EquityFunds.Add(_fundDetails);
                                else if (_destinationObjectName == "RealEstateFunds")
                                    _sectionContent.RealEstateFunds.Add(_fundDetails);
                                else if (_destinationObjectName == "MetalFunds")
                                    _sectionContent.MetalFunds.Add(_fundDetails);
                                else if (_destinationObjectName == "MixedFunds")
                                    _sectionContent.MixedFunds.Add(_fundDetails);
                                _posItem.AlreadyUsed = true;
                            }
                        }
                        _output.Content = new Section16And17Content(_sectionContent);
                    }
                }
            }
            return _output;
        }

    }

}
