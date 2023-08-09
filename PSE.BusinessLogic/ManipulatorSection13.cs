using System.Globalization;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using static PSE.Model.Common.Constants;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection13 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection13(CultureInfo? culture = null) : base(culture) { }

        public IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            Section13 _output = new()
            {
                SectionCode = OUTPUT_SECTION13_CODE,
                SectionName = "Bonds"
            };
            if (extractedData.Any(_flt => _flt.RecordType == nameof(IDE)) && extractedData.Any(_flt => _flt.RecordType == nameof(POS)))
            {
                IBondsMinorOrEqualTo1Year _bondMinOrEquTo1Year;
                ISection13Content _sectionContent;
                List<IDE> _ideItems = extractedData.Where(_flt => _flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> _posItems = extractedData.Where(_flt => _flt.RecordType == nameof(POS)).OfType<POS>().Where(_fltSubCat => _fltSubCat.SubCat4_15.Trim() == CODE_SUB_CATEGORY_SECTION13);
                foreach (IDE _ideItem in _ideItems)
                {
                    if (_posItems != null && _posItems.Any(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2))
                    {
                        _sectionContent = new Section13Content();
                        foreach (POS _posItem in _posItems)
                        {
                            _bondMinOrEquTo1Year = new BondsMinorOrEqualTo1Year()
                            {
                                ValorNumber = _posItem.NumSecurity_29 != null ? _posItem.NumSecurity_29 : 0,
                                Currency = _posItem.Currency1_17,
                                Description = ((string.IsNullOrEmpty(_posItem.Description1_32) ? "" : _posItem.Description1_32) + " " + (string.IsNullOrEmpty(_posItem.Description2_33) ? "" : _posItem.Description2_33)).Trim(),
                                Expiration = _posItem.MaturityDate_36 != null ? ((DateTime)_posItem.MaturityDate_36).ToString(DEFAULT_DATE_FORMAT, _culture) : "",
                                CurrentPrice = _posItem.Quote_48 != null ? _posItem.Quote_48.Value : 0,
                                PurchasePrice = _posItem.BuyPriceHistoric_53 != null ? _posItem.BuyPriceHistoric_53.Value : 0,
                                Isin = _posItem.IsinIban_85,
                                PriceBeginningYear = _posItem.BuyPriceAverage_87 != null ? _posItem.BuyPriceAverage_87.Value : 0,
                                PercentCoupon = _posItem.InterestRate_47 != null ? _posItem.InterestRate_47.Value : 0,
                                PercentYTM = 0, // not still recovered (!)
                                NominalAmount = _posItem.Quantity_28 != null ? _posItem.Quantity_28.Value : 0,
                                SPRating = "[SPRating]", // not still recovered (!)
                                MsciEsg = "[MsciEsg]", // not still recovered (!)
                                ExchangeRateImpactPurchase = _posItem.BuyExchangeRateHistoric_66 != null ? _posItem.BuyExchangeRateHistoric_66.Value : 0,
                                ExchangeRateImpactYTD = 0, // not still recovered (!)
                                PerformancePurchase = 0, // not still recovered (!)
                                PercentPerformancePurchase = 0, // not still recovered (!)
                                PerformanceYTD = 0, // not still recovered (!)
                                PercentPerformanceYTD = 0, // not still recovered (!)
                                PercentAsset = 0 // not still recovered (!)
                            };
                            _sectionContent.BondsMaturingMinorOrEqualTo1Year.Add(_bondMinOrEquTo1Year);
                        }
                        _output.Content = new Section13Content(_sectionContent);
                    }
                }
            }
            return _output;
        }

    }

}
