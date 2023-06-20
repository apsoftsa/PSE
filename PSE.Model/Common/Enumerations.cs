
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
            Failed = 1
        };

        public enum ManipolationTypes : int
        {
            Undefined = -1,
            AsSection3 = 3
        };

        public enum BuildFormats : int
        {
            Undefined = -1,
            Json = 0,
            Xml = 1
        };

    }

}
