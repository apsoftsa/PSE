using System.Globalization;
using PSE.Model.Input.Interfaces;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using static PSE.Model.Common.Constants;

namespace PSE.BusinessLogic
{

    public class ManipulatorHeader : ManipulatorBase, IManipulator
    {

        public ManipulatorHeader(CultureInfo? culture = null) : base(culture) { }

        public IOutputModel Manipulate(IList<IInputRecord> extractedData)
        {
            IHeaderContent _headerContent = new HeaderContent()
            {
                SectionCode = OUTPUT_HEADER_CODE,
                SectionName = "Headers",
                Logo = string.Empty,
                CompanyName = string.Empty
            };
            return _headerContent;
        }

    }

}
