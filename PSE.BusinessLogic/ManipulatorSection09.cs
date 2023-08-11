using System.Globalization;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using PSE.Model.SupportTables;
using static PSE.Model.Common.Constants;
using static PSE.Model.Common.Enumerations;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection9 : ManipulatorBase
    {

        public ManipulatorSection9(CultureInfo? culture = null) : base(PositionClassifications.INVESTIMENTI_BREVE_TERMINE, ManipolationTypes.AsSection9, culture) { }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            SectionBinding _sectionDest = Utility.ManipulatorOperatingRules.GetDestinationSection(this);
            Section9 _output = new()
            {
                SectionId = _sectionDest.SectionId,
                SectionCode = _sectionDest.SectionCode,
                SectionName = _sectionDest.SectionContent
            };
            if (extractedData.Any(_flt => _flt.RecordType == nameof(IDE)) && extractedData.Any(_flt => _flt.RecordType == nameof(POS)))
            {
                IShortTermInvestment _shortTermInvestiment;
                ISection9Content _sectionContent;
                List<IDE> _ideItems = extractedData.Where(_flt => _flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> _posItems = extractedData.Where(_flt => _flt.RecordType == nameof(POS)).OfType<POS>().Where(_fltSubCat => Utility.ManipulatorOperatingRules.IsRowDestinatedToManipulator(this, _fltSubCat.SubCat4_15));
                foreach (IDE _ideItem in _ideItems)
                {
                    if (_posItems != null && _posItems.Any(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2))
                    {
                        _sectionContent = new Section9Content();
                        foreach (POS _posItem in _posItems.Where(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2))
                        {
                            _shortTermInvestiment = new ShortTermInvestment()
                            {
                                Description = ((string.IsNullOrEmpty(_posItem.Description1_32) ? "" : _posItem.Description1_32) + " " + (string.IsNullOrEmpty(_posItem.Description2_33) ? "" : _posItem.Description2_33)).Trim(),
                                Currency = _posItem.Currency1_17,
                                NominalAmount = _posItem.Quantity_28 != null ? _posItem.Quantity_28.Value : 0,
                                ValorNumber = _posItem.NumSecurity_29 != null ? _posItem.NumSecurity_29.Value : 0,
                                CurrentPrice = _posItem.Quote_48 != null ? _posItem.Quote_48.Value : null,
                                PurchasePrice = _posItem.BuyPriceHistoric_53 != null ? _posItem.BuyPriceHistoric_53.Value : 0,
                                Isin = _posItem.IsinIban_85,
                                PriceBeginningYear = _posItem.BuyPriceAverage_87 != null ? _posItem.BuyPriceAverage_87.Value : 0,
                                DescriptionExtra = "[DescriptionExtra]", // not still recovered (!)
                                SPRating = "[SPRating]", // not still recovered (!)
                                MsciEsg = "[MsciEsg]", // not still recovered (!)
                                ExchangeRateImpactPurchase = 0, // not still recovered (!)
                                ExchangeRateImpactYTD = 0, // not still recovered (!)
                                PerformancePurchase = 0, // not still recovered (!)
                                PercentPerformancePurchase = 0, // not still recovered (!)
                                PerformanceYTD = 0, // not still recovered (!)
                                PercentPerformanceYTD = 0, // not still recovered (!)
                                PercentAsset = 0 // not still recovered (!)
                            };
                            _sectionContent.Investments.Add(_shortTermInvestiment);
                        }
                        _output.Content = new Section9Content(_sectionContent);
                    }
                }
            }
            return _output;
        }

    }

}
