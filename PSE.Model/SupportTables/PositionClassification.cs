using static PSE.Model.Common.Enumerations;

namespace PSE.Model.SupportTables
{

    public class PositionClassification
    {

        public PositionClassifications Code { get; }
        public string Description { get; }
        public int VisfAnn { get; }

        public PositionClassification()
        {
            Code = PositionClassifications.UNKNOWN;
            Description = string.Empty;
            VisfAnn = 0;
        }

        public PositionClassification(PositionClassifications code, string description, int visfAnn)
        {
            Code = code;
            Description = description;
            VisfAnn = visfAnn;
        }

    }

}
