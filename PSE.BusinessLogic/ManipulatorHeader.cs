using System.Globalization;
using PSE.Model.Common;
using PSE.Model.Input.Interfaces;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using PSE.Model.SupportTables;

namespace PSE.BusinessLogic
{

    public class ManipulatorHeader : ManipulatorBase
    {

        public ManipulatorHeader(CultureInfo? culture = null) : base(Enumerations.ManipolationTypes.AsHeader, culture) { }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            SectionBinding _sectionDest = Utility.SupportTablesIntegrator.GetDestinationSection(this);
            IHeaderContent _headerContent = new HeaderContent()
            {
                SectionId = _sectionDest.SectionId,
                SectionCode = _sectionDest.SectionCode,
                SectionName = _sectionDest.SectionContent,
                Logo = "[Logo]", // not still recovered (!)
                CompanyName = "[CompanyName]" // not still recovered (!)
            };
            return _headerContent;
        }

    }

}
