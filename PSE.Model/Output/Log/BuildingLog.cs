using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Enumerations;

namespace PSE.Model.Output.Log
{

    public class BuildingLog : IBuildingLog
    {

        public BuildingOutcomes Outcome { get; set; }
        public DateTime? BuildingStart { get; set; }
        public DateTime? BuildingEnd { get; set; }
        public Exception? ExceptionOccurred { get; set; }
        public string? FurtherErrorMessage { get; set; }

        public BuildingLog() 
        {
            Outcome = BuildingOutcomes.Unknown;
            BuildingStart = null;
            BuildingEnd = null;
            ExceptionOccurred = null;
            FurtherErrorMessage = string.Empty;
        }

        public BuildingLog(IBuildingLog source)
        {
            Outcome = source.Outcome;
            BuildingStart = source.BuildingStart;
            BuildingEnd = source .BuildingEnd;
            ExceptionOccurred = source.ExceptionOccurred;
            FurtherErrorMessage = source.FurtherErrorMessage;
        }

    }

}
