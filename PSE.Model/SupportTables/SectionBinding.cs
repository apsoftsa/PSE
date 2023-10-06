using static PSE.Model.Common.Enumerations;

namespace PSE.Model.SupportTables
{

    public class SectionBinding
    {

        public ManipolationTypes SectionId  { get; }
        public string SectionCode { get; }
        public string SectionContent { get; }
        public List<PositionClassifications> ClassificationsBound { get; }

        public SectionBinding()
        {
            SectionId = ManipolationTypes.Undefined;
            SectionCode = string.Empty;
            SectionContent = string.Empty;
            ClassificationsBound = new List<PositionClassifications>();
        }

        public SectionBinding(ManipolationTypes sectionId, string sectionCode, string sectionContent, List<PositionClassifications>? classificationsBound = null)
        {
            SectionId = sectionId;
            SectionCode = sectionCode;
            SectionContent = sectionContent;
            ClassificationsBound = classificationsBound != null && classificationsBound.Any() ? new List<PositionClassifications>(classificationsBound) : new List<PositionClassifications>();
        }

    }

}