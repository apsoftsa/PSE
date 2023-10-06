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

    public class ManipulatorFooter : ManipulatorBase, IManipulator
    {

        public ManipulatorFooter(CultureInfo? culture = null) : base(Enumerations.ManipolationTypes.AsFooter, culture) { }

        public override IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            SectionBinding _sectionDest = Utility.ManipulatorOperatingRules.GetDestinationSection(this);
            IFooterContent _footerContent = new FooterContent()
            {
                SectionId = _sectionDest.SectionId,
                SectionCode = _sectionDest.SectionCode,
                SectionName = _sectionDest.SectionContent,
                BankAddress = "[BankAddress]" // not still recovered (!)
            };
            return _footerContent;
        }

    }

}
