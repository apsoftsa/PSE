using System.Globalization;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using static PSE.Model.Common.Constants;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection12 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection12(CultureInfo? culture = null) : base(culture) { }

        public IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            Section12 _output = new Section12()
            {
                SectionCode = OUTPUT_SECTION12_CODE,
                SectionName = "Bonds-5-years"
            };
            if (extractedData.Any(_flt => _flt.RecordType == nameof(POS)))
            {
                IEnumerable<POS> _posItems = extractedData.Where(_flt => _flt.RecordType == nameof(POS)).OfType<POS>().Where(_fltSubCat => _fltSubCat.SubCat4_15.Trim() == CODE_SUB_CATEGORY_SECTION12);
                if (_posItems != null && _posItems.Any())
                {
                    IBondsMaturingLessThan5Years _bondLessThan5;
                    ISection12Content _sectionContent = new Section12Content();
                    foreach (POS _posItem in _posItems)
                    {
                        _bondLessThan5 = new BondsMaturingLessThan5Years()
                        {
                            ValorNumber = _posItem.NumSecurity_29,
                            Currency = _posItem.HostPositionCurrency_8,
                            Description = _posItem.Description1_32,
                            Expiration = _posItem.MaturityDate_36 != null ? ((DateTime)_posItem.MaturityDate_36).ToString("dd/MM/yyyy", _culture) : "",
                            CurrentPrice = _posItem.Quote_48,
                            PurchasePrice = _posItem.BuyPriceHistoric_53,
                            Isin = _posItem.IsinIban_85,
                            PriceAtTheBeginningOfTheYear = _posItem.BuyPriceAverage_87,
                            AmountNominal = "[AmountNominal]",
                            MsciEsg = "[MsciEsg]",
                            PercentAssets = "[PercentAssets]",
                            PercentCoupon = "[PercentCoupon]",
                            PercentImpactChangeFromPurchase = "[PercentImpactChangeFromPurchase]",
                            PercentImpactChangeYTD = "[PercentImpactChangeYTD]",
                            PercentPerformancefromPurchase = "[PercentPerformancefromPurchase]",
                            PercentPerformanceYTD = "[PercentPerformanceYTD]",
                            PercentYTM = "[PercentYTM]",
                            SPRating = "[SPRating]"
                        };
                        _sectionContent.BondsMaturingLessThan5Years.Add(_bondLessThan5);
                    }
                    _output.Content = new Section12Content(_sectionContent);
                }
            }
            return _output;
        }

    }

}
