using System.Globalization;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using static PSE.Model.Common.Constants;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection8 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection8(CultureInfo? culture = null) : base(culture) { }

        public IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            Section8 _output = new Section8()
            {
                SectionCode = OUTPUT_SECTION8_CODE,
                SectionName = "Liquidity"
            };
            if (extractedData.Any(_flt => _flt.RecordType == nameof(POS)))
            {
                IEnumerable<POS> _posItems = extractedData.Where(_flt => _flt.RecordType == nameof(POS)).OfType<POS>().Where(_fltSubCat => _fltSubCat.SubCat4_15.Trim() == CODE_SUB_CATEGORY_SECTION8);
                if (_posItems != null && _posItems.Any())
                {
                    IAccount _account;
                    ISection8Content _sectionContent = new Section8Content();
                    foreach (POS _posItem in _posItems)
                    {
                        _account = new Account()
                        {
                            AccountData = _posItem.HostPositionReference_6,
                            Currency = _posItem.HostPositionCurrency_8,
                            MarketValueReportingCurrency = _posItem.Amount1Cur1_22 != null ? _posItem.Amount1Cur1_22.Value.ToString(_culture) : "",
                            CurrentBalance = _posItem.Quantity_28 != null ? _posItem.Quantity_28.Value.ToString(_culture) : "",
                            Iban = _posItem.IsinIban_85,
                            AccruedInterestReportingCurrency = "[AccruedInterestReportingCurrency]",
                            ParentAssets = "[ParentAssets]"
                        };
                        _sectionContent.Accounts.Add(_account);
                    }
                    _output.Content = new Section8Content(_sectionContent);
                }                
            }
            return _output;
        }

    }

}
