using System.Globalization;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using static PSE.Model.Common.Constants;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection9 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection9(CultureInfo? culture = null) : base(culture) { }

        public IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            Section9 _output = new()
            {
                SectionCode = OUTPUT_SECTION9_CODE,
                SectionName = "SHORT-TERM INVESTMENTS"
            };
            if (extractedData.Any(_flt => _flt.RecordType == nameof(IDE)) && extractedData.Any(_flt => _flt.RecordType == nameof(POS)))
            {
                IShortTermInvestment _shortTermInvestiment;
                ISection9Content _sectionContent;
                List<IDE> _ideItems = extractedData.Where(_flt => _flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> _posItems = extractedData.Where(_flt => _flt.RecordType == nameof(POS)).OfType<POS>().Where(_fltSubCat => _fltSubCat.SubCat4_15.Trim() == CODE_SUB_CATEGORY_SECTION9);
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
                                CurrentPriceToPurchase = _posItem.Quote_48 != null ? _posItem.Quote_48.Value.ToString(_culture) : "",
                                PurchasePrice = _posItem.BuyPriceHistoric_53 != null ? _posItem.BuyPriceHistoric_53.Value.ToString(_culture) : "",
                                NominalAmount = _posItem.Quantity_28 != null ? _posItem.Quantity_28.Value.ToString(_culture) : "",
                                ValorNumber = _posItem.NumSecurity_29,
                                CurrentPriceYTD = "[CurrentPriceYTD]",
                                Isin = "[Isin]",
                                MsciEsg = "[MsciEsg]",
                                PercentAssets = "[PercentAssets]",
                                PercentImpactChangeToPurchase = "[PercentImpactChangeToPurchase]",
                                PercentImpactChangeYTD = "[PercentImpactChangeYTD]",
                                PercentPerformance = "[PercentPerformance]",
                                Performance = "[Performance]",
                                PriceAtTheBeginningOfTheYear = "[PriceAtTheBeginningOfTheYear]",
                                SAndPRating = "[SAndPRating]"
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
