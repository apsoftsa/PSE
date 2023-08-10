using static PSE.Model.Common.Enumerations;

namespace PSE.Model.SupportTables
{

    public class ValueTypology
    {

        public ValueTypologies Code { get; }
        public string Description { get; }

        public ValueTypology()
        {
            Code = ValueTypologies.UNKNOWN;
            Description = string.Empty;
        }

        public ValueTypology(ValueTypologies code, string description)
        {
            Code = code;
            Description = description;
        }

    }

}