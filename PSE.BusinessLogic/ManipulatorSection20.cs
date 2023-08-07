using System.Globalization;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using static PSE.Model.Common.Constants;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection20 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection20(CultureInfo? culture = null) : base(culture) { }

        public IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            Section20 _output = new()
            {
                SectionCode = OUTPUT_SECTION20_CODE,
                SectionName = "Metals"
            };
            if (extractedData.Any(_flt => _flt.RecordType == nameof(IDE)) && extractedData.Any(_flt => _flt.RecordType == nameof(POS)))
            {
                IMetalPhysicalMetalAccount _metPhyMetAcc;
                ISection20Content _sectionContent;
                List<IDE> _ideItems = extractedData.Where(_flt => _flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> _posItems = extractedData.Where(_flt => _flt.RecordType == nameof(POS)).OfType<POS>().Where(_fltSubCat => _fltSubCat.SubCat4_15.Trim() == CODE_SUB_CATEGORY_SECTION20);
                foreach (IDE _ideItem in _ideItems)
                {
                    if (_posItems != null && _posItems.Any(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2))
                    {
                        _sectionContent = new Section20Content();
                        foreach (POS _posItem in _posItems)
                        {
                            _metPhyMetAcc = new MetalPhysicalMetalAccount()
                            {
                                Account = _posItem.HostPositionReference_6,
                                CurrentBalance = _posItem.Amount1Cur1_22 != null ? _posItem.Amount1Cur1_22.Value : null,
                                MarketValueReportingCurrency = _posItem.Amount1Base_23 != null ? _posItem.Amount1Base_23.Value : null,
                                Amount = _posItem.Quantity_28 != null ? _posItem.Quantity_28.Value : null,
                                PurchasingCourse = _posItem.Quote_48 != null ? _posItem.Quote_48.Value : null
                            };
                            _sectionContent.MetalPhysicalMetalAccounts.Add(_metPhyMetAcc);
                        }
                        _output.Content = new Section20Content(_sectionContent);
                    }
                }
            }
            return _output;
        }

    }

}
