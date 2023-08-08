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
            Section12 _output = new()
            {
                SectionCode = OUTPUT_SECTION12_CODE,
                SectionName = "Bonds"
            };
            if (extractedData.Any(_flt => _flt.RecordType == nameof(IDE)) && extractedData.Any(_flt => _flt.RecordType == nameof(POS)))
            {
                IBondsMaturingLessThan5Years _bondLessThan5;
                ISection12Content _sectionContent;
                List<IDE> _ideItems = extractedData.Where(_flt => _flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> _posItems = extractedData.Where(_flt => _flt.RecordType == nameof(POS)).OfType<POS>().Where(_fltSubCat => _fltSubCat.SubCat4_15.Trim() == CODE_SUB_CATEGORY_SECTION12);
                foreach (IDE _ideItem in _ideItems)
                {
                    if (_posItems != null && _posItems.Any(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2))
                    {
                        _sectionContent = new Section12Content();
                        foreach (POS _posItem in _posItems)
                        {
                            _bondLessThan5 = new BondsMaturingLessThan5Years()
                            {
                                ValorNumber = _posItem.NumSecurity_29 != null ? _posItem.NumSecurity_29 : 0,
                                Currency = _posItem.Currency1_17,
                                Description = ((string.IsNullOrEmpty(_posItem.Description1_32) ? "" : _posItem.Description1_32) + " " + (string.IsNullOrEmpty(_posItem.Description2_33) ? "" : _posItem.Description2_33)).Trim(),
                                Expiration = _posItem.MaturityDate_36 != null ? ((DateTime)_posItem.MaturityDate_36).ToString(DEFAULT_DATE_FORMAT, _culture) : "",
                                CurrentPrice = _posItem.Quote_48 != null ? _posItem.Quote_48.Value : 0,
                                PurchasePrice = _posItem.BuyPriceHistoric_53 != null ? _posItem.BuyPriceHistoric_53.Value : 0,
                                Isin = _posItem.IsinIban_85,
                                PriceBeginningYear = _posItem.BuyPriceAverage_87 != null ? _posItem.BuyPriceAverage_87.Value : 0,
                                PercentCoupon = 0, // not still recovered (!)
                                PercentYTM = 0, // not still recovered (!)
                                NominalAmount = 0, // not still recovered (!)
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
                            _sectionContent.BondsMaturingLessThan5Years.Add(_bondLessThan5);
                        }
                        _output.Content = new Section12Content(_sectionContent);
                    }
                }
            }
            return _output;
        }

    }

}
