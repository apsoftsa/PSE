using System.Globalization;
using PSE.BusinessLogic.Common;
using PSE.BusinessLogic.Interfaces;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using PSE.Model.SupportTables;
using static PSE.Model.Common.Constants;
using static PSE.Model.Common.Enumerations;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection10 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection10(CultureInfo? culture = null) : base(PositionClassifications.INVESTIMENTI_FIDUCIARI, ManipolationTypes.AsSection10, culture) { }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            SectionBinding _sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            Section10 _output = new()
            {
                SectionId = _sectionDest.SectionId,
                SectionCode = _sectionDest.SectionCode,
                SectionName = _sectionDest.SectionContent
            };
            if (extractedData.Any(_flt => _flt.RecordType == nameof(IDE)) && extractedData.Any(_flt => _flt.RecordType == nameof(POS)))
            {
                IFiduciaryInvestmentAccount _account;
                ISection10Content _sectionContent;
                List<IDE> _ideItems = extractedData.Where(_flt => _flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> _posItems = extractedData.Where(_flt => _flt.AlreadyUsed == false && _flt.RecordType == nameof(POS)).OfType<POS>().Where(_fltSubCat => ManipulatorOperatingRules.IsRowDestinatedToManipulator(this, _fltSubCat.SubCat4_15));
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
                                MarketValueReportingCurrency = _posItem.Amount1Base_23 != null ? _posItem.Amount1Base_23.Value : 0,
                                FaceValue = _posItem.Quantity_28 != null ? _posItem.Quantity_28.Value : 0,
                                PercentInterest = _posItem.InterestRate_47 != null ? _posItem.InterestRate_47.Value : 0,
                                ExpirationDate = _posItem.MaturityDate_36 != null ? _posItem.MaturityDate_36.Value.ToString(DEFAULT_DATE_FORMAT, _culture) : "",
                                OpeningDate = _posItem.ConversionDateStart_41 != null ? _posItem.ConversionDateStart_41.Value.ToString(DEFAULT_DATE_FORMAT, _culture) : "",
                                NoDeposit = _posItem.MovementKey_31,
                                Correspondent = _posItem.Description2_33,
                                AccruedInterestReportingCurrency = _posItem.ProRataBase_56 != null ? _posItem.ProRataBase_56.Value : 0,
                                PercentAsset = 0 // not still recovered (!)
                            };
                            _sectionContent.Accounts.Add(_account);
                            _posItem.AlreadyUsed = true;
                        }
                        _output.Content = new Section10Content(_sectionContent);
                    }
                }
            }
            return _output;
        }

    }

}
