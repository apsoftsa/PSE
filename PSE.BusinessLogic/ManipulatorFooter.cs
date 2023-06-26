using System.Globalization;
using PSE.Model.Input.Interfaces;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using static PSE.Model.Common.Constants;

namespace PSE.BusinessLogic
{

    public class ManipulatorFooter : ManipulatorBase, IManipulator
    {

        public ManipulatorFooter(CultureInfo? culture = null) : base(culture) { }

        public IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            IFooterContent _footerContent = new FooterContent()
            {
                SectionCode = OUTPUT_FOOTER_CODE,
                SectionName = "Footers",
                BankAddress = "[BankAddress]"
            };
            return _footerContent;
        }

    }

}
