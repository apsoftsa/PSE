using System.Globalization;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using static PSE.Model.Common.Constants;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection14 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection14(CultureInfo? culture = null) : base(culture) { }

        public IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            Section14 _output = new()
            {
                SectionCode = OUTPUT_SECTION14_CODE,
                SectionName = "Bonds"
            };
            if (extractedData.Any(_flt => _flt.RecordType == nameof(IDE)) && extractedData.Any(_flt => _flt.RecordType == nameof(POS)))
            {
                IObligationsWithMaturityGreatherThanFiveYears _oblWithMatGreatThan5;
                ISection14Content _sectionContent;
                List<IDE> _ideItems = extractedData.Where(_flt => _flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> _posItems = extractedData.Where(_flt => _flt.RecordType == nameof(POS)).OfType<POS>().Where(_fltSubCat => _fltSubCat.SubCat4_15.Trim() == CODE_SUB_CATEGORY_SECTION14);
                foreach (IDE _ideItem in _ideItems)
                {
                    if (_posItems != null && _posItems.Any(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2))
                    {
                        _sectionContent = new Section14Content();
                        foreach (POS _posItem in _posItems)
                        {
                            _oblWithMatGreatThan5 = new ObligationsWithMaturityGreatherThanFiveYears()
                            {
                                ValorNumber = _posItem.NumSecurity_29,
                                Currency = _posItem.Currency1_17,
                                Description = _posItem.Description1_32,
                                CurrentPriceFromPurchase = _posItem.Quote_48 != null ? _posItem.Quote_48.Value.ToString(_culture) : "",
                                PurchasePrice = _posItem.BuyPriceHistoric_53 != null ? _posItem.BuyPriceHistoric_53.Value.ToString(_culture) : "",
                                PriceBeginningYear = _posItem.BuyPriceAverage_87,
                                Expiration = _posItem.MaturityDate_36 != null ? ((DateTime)_posItem.MaturityDate_36).ToString("dd.MM.yyyy", _culture) : "",
                                Isin = _posItem.IsinIban_85,
                                MsciEsg = "[MsciEsg]",
                                PercentAssets = "[PercentAssets]",
                                CurrentPriceFromYTD = "[CurrentPriceFromYTD]",
                                ExchangeRateImpactPurchase = "[ExchangeRateImpactPurchase]",
                                ExchangeRateImpactYTD = "[ExchangeRateImpactYTD]",
                                NominalAmount = "[NominalAmount]",
                                PercentCoupon = "[PercentCoupon]",
                                PercentYTM = "[PercentYTM]",
                                PerformanceYTD = "[PerformanceYTD]",
                                PerformancePurchase = "[PerformancePurchase]",
                                SPRating = "[SPRating]"
                            };
                            _sectionContent.ObligationsWithMaturityGreatherThanFiveYears.Add(_oblWithMatGreatThan5);
                        }
                        _output.Content = new Section14Content(_sectionContent);
                    }
                }
            }
            return _output;
        }

    }

}
