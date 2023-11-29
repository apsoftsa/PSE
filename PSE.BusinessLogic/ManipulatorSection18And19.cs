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

    public class ManipulatorSection18And19 : ManipulatorBase, IManipulator
    {

        private readonly OtherInvestmentsCalculation _calcOtherInvs;

        public ManipulatorSection18And19(CalculationSettings calcSettings, CultureInfo? culture = null) : base(new List<PositionClassifications>()
            {
                PositionClassifications.PRODOTTI_DERIVATI_SU_METALLI, 
                PositionClassifications.PRODOTTI_DERIVATI, 
                PositionClassifications.PRODOTTI_ALTERNATIVI_DIVERSI
            }, 
            ManipolationTypes.AsSection18And19, culture) 
        {
            _calcOtherInvs = new OtherInvestmentsCalculation(calcSettings);
        }

        public override string GetObjectNameDestination(IInputRecord inputRecord)
        {
            string _destinationObjectName = String.Empty;
            if (inputRecord != null && inputRecord.GetType() == typeof(POS))
            {
                string _subCategory = ((POS)inputRecord).SubCat4_15;
                if (_subCategory != null && Enum.IsDefined(typeof(PositionClassifications), int.Parse(_subCategory)))
                {
                    if (this.PositionClassificationsSource.Contains((PositionClassifications)int.Parse(_subCategory)))
                    {
                        switch ((PositionClassifications)int.Parse(_subCategory))
                        {
                            case PositionClassifications.PRODOTTI_DERIVATI_SU_METALLI:
                                _destinationObjectName = "DerivativesOnMetals";
                                break;
                            case PositionClassifications.PRODOTTI_DERIVATI:
                            case PositionClassifications.PRODOTTI_ALTERNATIVI_DIVERSI:
                                _destinationObjectName = "Different";
                                break;
                        }
                    }
                }
            }
            return _destinationObjectName;
        }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            SectionBinding _sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section18And19 _output = new()
            {
                SectionId = _sectionDest.SectionId,
                SectionCode = _sectionDest.SectionCode,
                SectionName = _sectionDest.SectionContent
            };
            if (extractedData.Any(_flt => _flt.RecordType == nameof(IDE)) && extractedData.Any(_flt => _flt.RecordType == nameof(POS)))
            {
                decimal _currencyRate, _customerSumAmounts, _currentBaseValue, _quoteType;
                string _destinationObjectName;
                IAlternativeProductDetail _altProdDetails;
                IAlternativeProducts _altProdDefinitions;
                ISection18And19Content _sectionContent;
                List<IDE> _ideItems = extractedData.Where(_flt => _flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> _posItems = extractedData.Where(_flt => _flt.AlreadyUsed == false && _flt.RecordType == nameof(POS)).OfType<POS>().Where(_fltSubCat => ManipulatorOperatingRules.IsRowDestinatedToManipulator(this, _fltSubCat.SubCat4_15));
                IEnumerable<CUR> _curItems = extractedData.Where(_flt => _flt.RecordType == nameof(CUR)).OfType<CUR>();
                foreach (IDE _ideItem in _ideItems)
                {
                    _customerSumAmounts = extractedData.Where(_flt => _flt.RecordType == nameof(POS)).OfType<POS>().Where(_subFlt => _subFlt.CustomerNumber_2 == _ideItem.CustomerNumber_2 && _subFlt.Amount1Base_23.HasValue).Sum(_sum => _sum.Amount1Base_23.Value);
                    _customerSumAmounts += extractedData.Where(_flt => _flt.RecordType == nameof(POS)).OfType<POS>().Where(_subFlt => _subFlt.CustomerNumber_2 == _ideItem.CustomerNumber_2 && _subFlt.ProRataBase_56.HasValue).Sum(_sum => _sum.ProRataBase_56.Value);
                    if (_posItems != null && _posItems.Any(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2))
                    {
                        _sectionContent = new Section18And19Content();
                        _altProdDefinitions = new AlternativeProducts();
                        foreach (POS _posItem in _posItems.Where(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2))
                        {
                            if ((_destinationObjectName = GetObjectNameDestination(_posItem)) != string.Empty)
                            {
                                _currentBaseValue = _posItem.Amount1Base_23.HasValue ? _posItem.Amount1Base_23.Value : 0;
                                _currentBaseValue += _posItem.ProRataBase_56.HasValue ? _posItem.ProRataBase_56.Value : 0;
                                _quoteType = string.IsNullOrEmpty(_posItem.QuoteType_51) == false && _posItem.QuoteType_51.Trim() == "%" ? 100m : 1m;
                                _altProdDetails = new AlternativeProductDetail()
                                {
                                    ValorNumber = _posItem.NumSecurity_29 != null ? _posItem.NumSecurity_29 : 0,
                                    Currency = _posItem.Currency1_17,
                                    Description = ((string.IsNullOrEmpty(_posItem.Description1_32) ? "" : _posItem.Description1_32) + " " + (string.IsNullOrEmpty(_posItem.Description2_33) ? "" : _posItem.Description2_33)).Trim(),
                                    DescriptionExtra = _posItem.CallaDate_38 != null ? ((DateTime)_posItem.CallaDate_38).ToString(DEFAULT_DATE_FORMAT, _culture) : "",
                                    CurrentPrice = _posItem.Quote_48 != null ? _posItem.Quote_48.Value : 0,
                                    PurchasePrice = _posItem.BuyPriceHistoric_53 != null ? _posItem.BuyPriceHistoric_53.Value : 0,
                                    Isin = _posItem.IsinIban_85,
                                    PriceBeginningYear = _posItem.BuyPriceAverage_87 != null ? _posItem.BuyPriceAverage_87.Value : 0,
                                    NominalAmount = _posItem.Quantity_28 != null ? _posItem.Quantity_28.Value : 0,
                                    UnderlyingDescription = _posItem.ConversionDesc_45,
                                    ExchangeRateImpactPurchase = _posItem.BuyExchangeRateHistoric_66 != null ? _posItem.BuyExchangeRateHistoric_66.Value : 0, // temporary
                                    ExchangeRateImpactYTD = _posItem.BuyExchangeRateAverage_88 != null ? _posItem.BuyExchangeRateAverage_88.Value : 0, // temporary
                                    PercentAsset = _customerSumAmounts != 0 && _currentBaseValue != 0 ? Math.Round(_currentBaseValue / _customerSumAmounts * 100m, _calcOtherInvs.MeaningfulDecimalDigits) : 0
                                };
                                _currencyRate = (_curItems != null && _curItems.Any(_flt => _flt.CustomerNumber_2 == _posItem.CustomerNumber_2 && _flt.Currency_5 == _altProdDetails.Currency && _flt.Rate_6 != null)) ? _curItems.First(_flt => _flt.CustomerNumber_2 == _posItem.CustomerNumber_2 && _flt.Currency_5 == _altProdDetails.Currency && _flt.Rate_6 != null).Rate_6.Value : 0;
                                _altProdDetails.PerformancePurchase = Math.Round((_altProdDetails.CurrentPrice.Value - _altProdDetails.PurchasePrice.Value) * _altProdDetails.NominalAmount.Value / _quoteType, _calcOtherInvs.MeaningfulDecimalDigits);
                                _altProdDetails.PerformanceYTD = Math.Round((_altProdDetails.CurrentPrice.Value - _altProdDetails.PriceBeginningYear.Value) * _altProdDetails.NominalAmount.Value / _quoteType, _calcOtherInvs.MeaningfulDecimalDigits);
                                _altProdDetails.PercentPerformancePurchase = _calcOtherInvs.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcOtherInvs.GetSign(_altProdDetails.NominalAmount, _altProdDetails.CurrentPrice), _altProdDetails.PurchasePrice.Value, _altProdDetails.CurrentPrice.Value));
                                _altProdDetails.PercentPerformanceYTD = _calcOtherInvs.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcOtherInvs.GetSign(_altProdDetails.NominalAmount, _altProdDetails.CurrentPrice), _altProdDetails.PriceBeginningYear.Value, _altProdDetails.CurrentPrice.Value));
                                _altProdDetails.ExchangeRateImpactPurchase = _calcOtherInvs.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcOtherInvs.GetSign(_altProdDetails.NominalAmount, _altProdDetails.CurrentPrice), _altProdDetails.ExchangeRateImpactPurchase.Value, _currencyRate));
                                _altProdDetails.ExchangeRateImpactYTD = _calcOtherInvs.GetPriceDifferenceValue(new PriceDifferenceValueParams(_calcOtherInvs.GetSign(_altProdDetails.NominalAmount, _altProdDetails.CurrentPrice), _altProdDetails.ExchangeRateImpactYTD.Value, _currencyRate));
                                if (_destinationObjectName == "Different")
                                    _altProdDefinitions.Different.Add(_altProdDetails);
                                else if (_destinationObjectName == "DerivativesOnMetals")
                                    _altProdDefinitions.DerivativesOnMetals.Add(_altProdDetails);
                                _posItem.AlreadyUsed = true;
                            }
                        }                                                
                        _sectionContent.AlternativeProducts.Add(_altProdDefinitions);
                        _output.Content = new Section18And19Content(_sectionContent);
                    }
                }
            }
            return _output;
        }

    }

}
