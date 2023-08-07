using System.Globalization;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using static PSE.Model.Common.Constants;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection10 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection10(CultureInfo? culture = null) : base(culture) { }

        public IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            Section10 _output = new()
            {
                SectionCode = OUTPUT_SECTION10_CODE,
                SectionName = "FIDUCIARY INVESTMENTS"
            };
            if (extractedData.Any(_flt => _flt.RecordType == nameof(IDE)) && extractedData.Any(_flt => _flt.RecordType == nameof(POS)))
            {
                IFiduciaryInvestmentAccount _account;
                ISection10Content _sectionContent;
                List<IDE> _ideItems = extractedData.Where(_flt => _flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> _posItems = extractedData.Where(_flt => _flt.RecordType == nameof(POS)).OfType<POS>().Where(_fltSubCat => _fltSubCat.SubCat4_15.Trim() == CODE_SUB_CATEGORY_SECTION10);
                foreach (IDE _ideItem in _ideItems)
                {
                    if (_posItems != null && _posItems.Any(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2))
                    {
                        _sectionContent = new Section10Content();
                        foreach (POS _posItem in _posItems.Where(_flt => _flt.CustomerNumber_2 == _ideItem.CustomerNumber_2))
                        {
                            _account = new FiduciaryInvestmentAccount()
                            {
                                Account = _posItem.HostPositionReference_6,
                                Currency = _posItem.Currency1_17,
                                MarketValueReportingCurrency = _posItem.Amount1Base_23 != null ? _posItem.Amount1Base_23.Value : null,
                                FaceValue = _posItem.Quantity_28 != null ? _posItem.Quantity_28.Value : null,
                                PercentInterest = _posItem.InterestRate_47 != null ? _posItem.InterestRate_47.Value : null,
                                ExpirationDate = _posItem.MaturityDate_36 != null ? _posItem.MaturityDate_36.Value.ToString("dd.MM.yyyy") : "",
                                OpeningDate = _posItem.ConversionDateStart_41 != null ? _posItem.ConversionDateStart_41.Value.ToString("dd.MM.yyyy") : "",
                                NoDeposit = _posItem.MovementKey_31,
                                Correspondent = _posItem.Description2_33,
                                AccruedInterestReportingCurrency = _posItem.ProRataBase_56 != null ? _posItem.ProRataBase_56.Value : null
                            };
                            _sectionContent.Accounts.Add(_account);
                        }
                        _output.Content = new Section10Content(_sectionContent);
                    }
                }
            }
            return _output;
        }

    }

}
