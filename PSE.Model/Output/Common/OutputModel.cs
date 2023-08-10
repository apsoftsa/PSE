using Newtonsoft.Json;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Enumerations;

namespace PSE.Model.Output.Common
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public abstract class OutputModel : IOutputModel
    {

        [JsonIgnore]
        public ManipolationTypes SectionId { get; set; }

        [JsonIgnore]
        public string SectionCode { get; set; }

        [JsonIgnore]
        public string SectionName { get; set; }

        protected OutputModel()
        {
            SectionId = ManipolationTypes.Undefined;
            SectionCode = string.Empty;
            SectionName = string.Empty;
        }

        protected OutputModel(IOutputModel source)
        {
            SectionId = source.SectionId;
            SectionCode = source.SectionCode;
            SectionName = source.SectionName;
        }

    }

}
