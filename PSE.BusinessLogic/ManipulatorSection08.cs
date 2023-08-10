using System.Globalization;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using PSE.Model.SupportTables;
using static PSE.Model.Common.Enumerations;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection8 : ManipulatorBase
    {

        public ManipulatorSection8(CultureInfo? culture = null) : base(PositionClassifications.CONTI, ManipolationTypes.AsSection8, culture) { }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            SectionBinding _sectionDest = Utility.SupportTablesIntegrator.GetDestinationSection(this);
            Section8 _output = new()
            {
                SectionId = _sectionDest.SectionId,
                SectionCode = _sectionDest.SectionCode,
                SectionName = _sectionDest.SectionContent
            };
            if (extractedData.Any(_flt => _flt.RecordType == nameof(IDE)) && extractedData.Any(_flt => _flt.RecordType == nameof(POS)))
            {
                IAccount _account;
                ISection8Content _sectionContent;
                List<IDE> _ideItems = extractedData.Where(_flt => _flt.RecordType == nameof(IDE)).OfType<IDE>().ToList();
                IEnumerable<POS> _posItems = extractedData.Where(_flt => _flt.RecordType == nameof(POS)).OfType<POS>().Where(_fltSubCat => Utility.SupportTablesIntegrator.IsDestinatedToManipulator(this, _fltSubCat.SubCat4_15));
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
                                MarketValueReportingCurrency = _posItem.Amount1Base_23 != null ? _posItem.Amount1Base_23.Value : 0,
                                CurrentBalance = _posItem.Quantity_28 != null ? _posItem.Quantity_28.Value : 0,
                                Iban = _posItem.IsinIban_85,
                                AccruedInterestReportingCurrency = 0, // not still recovered (!)
                                PercentAssets = 0 // not still recovered (!)
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
