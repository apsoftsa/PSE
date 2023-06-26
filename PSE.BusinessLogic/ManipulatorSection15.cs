using System.Globalization;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using static PSE.Model.Common.Constants;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection15 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection15(CultureInfo? culture = null) : base(culture) { }

        public IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            Section15 _output = new()
            {
                SectionCode = OUTPUT_SECTION15_CODE,
                SectionName = "Shares"
            };
            if (extractedData.Any(_flt => _flt.RecordType == nameof(IDE)) && extractedData.Any(_flt => _flt.RecordType == nameof(POS)))
            {
                IBondsWithMaturityGreatherThanFiveYears _bondGreatThan5;
                ISection15Content _sectionContent;
                List<IDE> _ideItems = extractedData.Where(_flt => _flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> _posItems = extractedData.Where(_flt => _flt.RecordType == nameof(POS)).OfType<POS>().Where(_fltSubCat => _fltSubCat.SubCat4_15.Trim() == CODE_SUB_CATEGORY_SECTION15);
                foreach (IDE _ideItem in _ideItems)
                {
                    if (_posItems != null && _posItems.Any(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2))
                    {
                        _sectionContent = new Section15Content();
                        foreach (POS _posItem in _posItems)
                        {
                            _bondGreatThan5 = new BondsWithMaturityGreatherThanFiveYears()
                            {
                                ValorNumber = _posItem.NumSecurity_29,
                                Currency = _posItem.HostPositionCurrency_8,
                                Description = _posItem.Description1_32,
                                CurrentPriceFromPurchase = _posItem.Quote_48,
                                PurchasePrice = _posItem.BuyPriceHistoric_53,
                                Isin = _posItem.IsinIban_85,
                                PriceBeginningYear = _posItem.BuyPriceAverage_87,
                                MsciEsg = "[MsciEsg]",
                                PercentAssets = "[PercentAssets]",
                                CurrentPriceFromYTD = "[CurrentPriceFromYTD]",
                                ExchangeRateImpactPurchase = "[ExchangeRateImpactPurchase]",
                                ExchangeRateImpactYTD = "[ExchangeRateImpactYTD]",
                                NominalAmount = "[NominalAmount]",
                                PercentperformanceYTD = "[PercentperformanceYTD]",
                                PerformancePurchase = "[PerformancePurchase]",
                                SPRating = "[SPRating]"
                            };
                            _sectionContent.BondsWithMatGreatThanFiveYears.Add(_bondGreatThan5);
                        }
                        _output.Content = new Section15Content(_sectionContent);
                    }
                }
            }
            return _output;
        }

    }

}
