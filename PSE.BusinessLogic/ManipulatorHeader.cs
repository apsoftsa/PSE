using System.Globalization;
using PSE.BusinessLogic.Common;
using PSE.BusinessLogic.Interfaces;
using PSE.Model.Common;
using PSE.Model.Events;
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
            IDE? ideItem = null;
            string customerName = string.Empty; 
            SectionBinding sectionDest = ManipulatorOperatingRules.GetDestinationSection(this);
            if (extractedData.Any(flt => flt.RecordType == nameof(IDE)))
            {
                ideItem = extractedData.Where(flt => flt.RecordType == nameof(IDE)).OfType<IDE>().FirstOrDefault();
                customerName = ideItem!.CustomerNameShort_5.Replace(" ", "_");
            }
            IHeaderContent headerContent = new HeaderContent()
            {
                SectionId = sectionDest.SectionId,
                SectionCode = sectionDest.SectionCode,
                SectionName = sectionDest.SectionContent,
                ReferenceString1 = ideItem != null ? ideItem.CustomerId_6 : string.Empty,
                ReferenceString2 = customerName,
                ReferenceString3 = ideItem != null && ideItem.Date_15 != null ? ideItem.Date_15.Value.ToString(COMPACT_DATE_FORMAT, _culture) : string.Empty
            };
            return headerContent;
        }

    }

}
