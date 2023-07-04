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
            Section8 _output = new()
            {
                SectionCode = OUTPUT_SECTION8_CODE,
                SectionName = "Liquidity"
            };
            if (extractedData.Any(_flt => _flt.RecordType == nameof(IDE)) && extractedData.Any(_flt => _flt.RecordType == nameof(POS)))
            {
                IAccount _account;
                ISection8Content _sectionContent;
                List<IDE> _ideItems = extractedData.Where(_flt => _flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> _posItems = extractedData.Where(_flt => _flt.RecordType == nameof(POS)).OfType<POS>().Where(_fltSubCat => _fltSubCat.SubCat4_15.Trim() == CODE_SUB_CATEGORY_SECTION8);
                foreach (IDE _ideItem in _ideItems)
                {
                    if (_posItems != null && _posItems.Any(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2))
                    {
                        _sectionContent = new Section8Content();
                        foreach (POS _posItem in _posItems.Where(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2))
                        {
                            _account = new Account()
                            {
                                AccountData = _posItem.HostPositionReference_6,
                                Currency = _posItem.Currency1_17,
                                MarketValueReportingCurrency = _posItem.Amount1Base_23 != null ? _posItem.Amount1Base_23.Value.ToString(_culture) : "",
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
            }
            return _output;
        }

    }

}
