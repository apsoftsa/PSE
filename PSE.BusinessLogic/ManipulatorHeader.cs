using System.Globalization;
using PSE.BusinessLogic.Common;
using PSE.BusinessLogic.Interfaces;
using PSE.Model.Common;
using PSE.Model.Input.Interfaces;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using PSE.Model.SupportTables;

namespace PSE.BusinessLogic
{

    public class ManipulatorHeader : ManipulatorBase, IManipulator
    {

        public ManipulatorHeader(CultureInfo? culture = null) : base(Enumerations.ManipolationTypes.AsHeader, culture) { }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            IHeaderContent headerContent = new HeaderContent()
            {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent,
                Logo = "[Logo]", // not still recovered (!)
                CompanyName = "[CompanyName]" // not still recovered (!)
            };
            return headerContent;
        }

    }

}
