using System.Globalization;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using static PSE.Model.Common.Constants;

namespace PSE.BusinessLogic
{

    public class ManipulatorSection1 : ManipulatorBase, IManipulator
    {

        public ManipulatorSection1(CultureInfo? culture = null) : base(culture) { }

        public IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            Section1 _output = new Section1()
            {
                SectionCode = OUTPUT_SECTION1_CODE,
                SectionName = "Asset Statement"
            };
            if (extractedData.Any(_flt => _flt.RecordType == nameof(IDE)))
            {
                IDE _ideItem = extractedData.Where(_flt => _flt.RecordType == nameof(IDE)).OfType<IDE>().First();
                ISection1Content _sectionContent = new Section1Content();
                IAssetStatement _assetStatement = new AssetStatement()
                {
                    Portfolio = _ideItem.CustomerNumber_2,
                    Advisor = _ideItem.Manager_8,
                    Customer = _ideItem.CustomerNameShort_5,
                    Date = _ideItem.Date_15 != null ? ((DateTime)_ideItem.Date_15).ToString("dd/MM/yyyy", _culture) : string.Empty
                };
                _sectionContent.AssetStatements.Add(_assetStatement);
                _output.Content = new Section1Content(_sectionContent);
            }
            return _output;
        }

    }

}
