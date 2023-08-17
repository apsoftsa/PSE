using System.Globalization;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using PSE.Model.SupportTables;
using static PSE.Model.Common.Enumerations;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection16And17 : ManipulatorBase
    {

        public ManipulatorSection16And17(CultureInfo? culture = null) : base(new List<PositionClassifications>()
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
        { }

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
            SectionBinding _sectionDest = Utility.ManipulatorOperatingRules.GetDestinationSection(this);
            Section16And17 _output = new()
            {
                SectionId = _sectionDest.SectionId,
                SectionCode = _sectionDest.SectionCode,
                SectionName = _sectionDest.SectionContent
            };
            if (extractedData.Any(_flt => _flt.RecordType == nameof(IDE)) && extractedData.Any(_flt => _flt.RecordType == nameof(POS)))
            {
                string _destinationObjectName;
                IFundDetails _fundDetails;
                ISection16And17Content _sectionContent;
                List<IDE> _ideItems = extractedData.Where(_flt => _flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> _posItems = extractedData.Where(_flt => _flt.AlreadyUsed == false && _flt.RecordType == nameof(POS)).OfType<POS>().Where(_fltSubCat => Utility.ManipulatorOperatingRules.IsRowDestinatedToManipulator(this, _fltSubCat.SubCat4_15));
                foreach (IDE _ideItem in _ideItems)
                {
                    if (_posItems != null && _posItems.Any(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2))
                    {
                        _sectionContent = new Section16And17Content();
                        foreach (POS _posItem in _posItems)
                        {
                            if ((_destinationObjectName = GetObjectNameDestination(_posItem)) != string.Empty)
                            {
                                _fundDetails = new FundDetail()
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
                                    CurrentPrice = _posItem.Quote_48 != null ? _posItem.Quote_48.Value : 0,
                                    PurchasePrice = _posItem.BuyPriceHistoric_53 != null
                                        ? _posItem.BuyPriceHistoric_53.Value
                                        : 0,
                                    PriceBeginningYear = _posItem.BuyPriceAverage_87 != null
                                        ? _posItem.BuyPriceAverage_87.Value
                                        : 0,
                                    NominalAmount = _posItem.Quantity_28 != null ? _posItem.Quantity_28.Value : 0,
                                    ExchangeRateImpactPurchase = _posItem.BuyExchangeRateHistoric_66 != null ? _posItem.BuyExchangeRateHistoric_66.Value : 0,
                                    Isin = _posItem.IsinIban_85,
                                    SPRating = (string.IsNullOrEmpty(_posItem.AgeRat_97) == false && _posItem.AgeRat_97.Trim() == "SP") ? _posItem.Rating_98 : string.Empty,
                                    MsciEsg = (string.IsNullOrEmpty(_posItem.AgeRat_97) == false && _posItem.AgeRat_97.Trim() == "ES") ? _posItem.Rating_98 : string.Empty,
                                    ExchangeRateImpactYTD = 0, // not still recovered (!)
                                    PerformancePurchase = 0, // not still recovered (!)
                                    PercentPerformancePurchase = 0, // not still recovered (!)
                                    PerformanceYTD = 0, // not still recovered (!)
                                    PercentPerformanceYTD = 0, // not still recovered (!)
                                    PercentAsset = 0 // not still recovered (!)
                                };
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
