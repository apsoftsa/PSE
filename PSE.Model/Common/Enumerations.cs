
namespace PSE.Model.Common
{

    public static class Enumerations
    {
        
        public enum StreamAcquisitionOutcomes : int
        {
            Unknown = -1,
            Success = 0,
            WithErrors = 1,
            Aborted = 2
        };

        public enum BuildingOutcomes : int
        {
            Unknown = -1,
            Success = 0,
            Failed = 1,
            Ignored = 2
        };

        public enum ManipolationTypes : int
        {
            Undefined = -1,
            AsHeader = 0,
            AsSection1 = 1,
            AsSection3 = 3,
            AsSection4 = 4,
            AsSection8 = 8,
            AsSection9 = 9,
            AsSection10 = 10,
            AsSection11 = 11,
            AsSection12 = 12,
            AsSection13 = 13,
            AsSection14 = 14,
            AsSection15 = 15,
            AsSection16And17 = 1617,
            AsSection18And19 = 1819,
            AsSection20 = 20,
            AsFooter = 99
        };

        public enum BuildFormats : int
        {
            Undefined = -1,
            Json = 0,
            Xml = 1
        };

    }

}
