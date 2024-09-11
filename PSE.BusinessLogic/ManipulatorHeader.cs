using System.Globalization;
using PSE.BusinessLogic.Common;
using PSE.BusinessLogic.Interfaces;
using PSE.Model.Common;
using PSE.Model.Input.Interfaces;
using PSE.Model.Input.Models;
using PSE.Model.Output.Interfaces;
using PSE.Model.Output.Models;
using PSE.Model.SupportTables;
using static PSE.Model.Common.Constants;

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
                CompanyName = "[CompanyName]", // not still recovered (!)
                RequestUUID = string.Empty
            };
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)))
            {
                IDE? ideItem = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().FirstOrDefault();
                if (ideItem != null)
                {
                    headerContent.Description.First().Settled = ideItem.Date_15 != null ? new List<ISettled>() { new Settled(ideItem.Date_15.Value.ToString(DEFAULT_DATE_FORMAT, _culture), ideItem.Time_16.Value.ToString(DEFAULT_TIME_FORMAT, _culture)) } : null;
                    headerContent.Description.First().CustomerID = ideItem.CustomerId_6;
                }
            }
            return headerContent;
        }

    }

}
