using System.Globalization;
using PSE.BusinessLogic.Common;
using PSE.BusinessLogic.Interfaces;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using PSE.Model.SupportTables;
using static PSE.Model.Common.Constants;
using static PSE.Model.Common.Enumerations;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection18And19 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection18And19(CultureInfo? culture = null) : base(new List<PositionClassifications>()
            {
                PositionClassifications.PRODOTTI_DERIVATI_SU_METALLI, 
                PositionClassifications.PRODOTTI_DERIVATI, 
                PositionClassifications.PRODOTTI_ALTERNATIVI_DIVERSI
            }
            , ManipolationTypes.AsSection18And19, culture) { }

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
            SectionBinding _sectionDest = Utility.ManipulatorOperatingRules.GetDestinationSection(this);
            Section18And19 _output = new()
            {
                SectionId = _sectionDest.SectionId,
                SectionCode = _sectionDest.SectionCode,
                SectionName = _sectionDest.SectionContent
            };
            if (extractedData.Any(_flt => _flt.RecordType == nameof(IDE)) && extractedData.Any(_flt => _flt.RecordType == nameof(POS)))
            {
                string _destinationObjectName;
                IAlternativeProductDetail _altProdDetails;
                IAlternativeProducts _altProdDefinitions;
                ISection18And19Content _sectionContent;
                List<IDE> _ideItems = extractedData.Where(_flt => _flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> _posItems = extractedData.Where(_flt => _flt.AlreadyUsed == false && _flt.RecordType == nameof(POS)).OfType<POS>().Where(_fltSubCat => Utility.ManipulatorOperatingRules.IsRowDestinatedToManipulator(this, _fltSubCat.SubCat4_15));
                foreach (IDE _ideItem in _ideItems)
                {
                    if (_posItems != null && _posItems.Any(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2))
                    {
                        _sectionContent = new Section18And19Content();
                        _altProdDefinitions = new AlternativeProducts();
                        foreach (POS _posItem in _posItems)
                        {
                            if ((_destinationObjectName = GetObjectNameDestination(_posItem)) != string.Empty)
                            {
                                _altProdDetails = new AlternativeProductDetail()
                                {
                                    ValorNumber = _posItem.NumSecurity_29 != null ? _posItem.NumSecurity_29 : 0,
                                    Currency = _posItem.Currency1_17,
                                    Description =
                                        ((string.IsNullOrEmpty(_posItem.Description1_32)
                                             ? ""
                                             : _posItem.Description1_32) + " " +
                                         (string.IsNullOrEmpty(_posItem.Description2_33)
                                             ? ""
                                             : _posItem.Description2_33)).Trim(),
                                    DescriptionExtra = _posItem.CallaDate_38 != null
                                        ? ((DateTime)_posItem.CallaDate_38).ToString(DEFAULT_DATE_FORMAT, _culture)
                                        : "",
                                    CurrentPrice = _posItem.Quote_48 != null ? _posItem.Quote_48.Value : 0,
                                    PurchasePrice = _posItem.BuyPriceHistoric_53 != null
                                        ? _posItem.BuyPriceHistoric_53.Value
                                        : 0,
                                    Isin = _posItem.IsinIban_85,
                                    PriceBeginningYear = _posItem.BuyPriceAverage_87 != null
                                        ? _posItem.BuyPriceAverage_87.Value
                                        : 0,
                                    NominalAmount = _posItem.Quantity_28 != null ? _posItem.Quantity_28.Value : 0,
                                    UnderlyingDescription = _posItem.ConversionDesc_45,
                                    ExchangeRateImpactPurchase = 0, // not still recovered (!)
                                    ExchangeRateImpactYTD = 0, // not still recovered (!)
                                    PerformancePurchase = 0, // not still recovered (!)
                                    PercentPerformancePurchase = 0, // not still recovered (!)
                                    PerformanceYTD = 0, // not still recovered (!)
                                    PercentPerformanceYTD = 0, // not still recovered (!)
                                    PercentAsset = 0 // not still recovered (!)
                                    // cambio storico ?! (!!!!)
                                };
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
