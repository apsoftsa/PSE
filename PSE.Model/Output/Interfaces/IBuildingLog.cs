using static PSE.Model.Common.Enumerations;

namespace PSE.Model.Output.Interfaces
{

    public interface IBuildingLog
    {

        BuildingOutcomes Outcome { get; set; }
        DateTime? BuildingStart { get; set; }
        DateTime? BuildingEnd { get; set; }
        Exception? ExceptionOccurred { get; set; }
        string? FurtherErrorMessage { get; set; }

    }

}
