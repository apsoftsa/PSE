using PSE.Model.Output.Log;
using PSE.Model.Output.Interfaces;

namespace PSE.Model.Output
{

    public class BuiltData : IBuiltData
    {

        public IBuildingLog BuildingLog { get; set; }
        public string OutputData { get; set; }

        public BuiltData() 
        {
            BuildingLog = new BuildingLog();
            OutputData = string.Empty;
        }

        public BuiltData(IBuiltData source)
        {
            BuildingLog = source.BuildingLog!= null ? new BuildingLog(source.BuildingLog) : new BuildingLog();
            OutputData = source.OutputData;
        }

    }

}
