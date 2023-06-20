using Newtonsoft.Json;
using PSE.Model.Output.Interfaces;

namespace PSE.Model.Output.Common
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public abstract class OutputModel : IOutputModel
    {

        [JsonIgnore]
        public string SectionCode { get; set; }

        [JsonIgnore]
        public string SectionName { get; set; }

        protected OutputModel()
        {
            SectionCode = string.Empty;
            SectionName = string.Empty;
        }

        protected OutputModel(string sectionCode) 
        { 
            SectionCode = sectionCode;
            SectionName = string.Empty; 
        }

        protected OutputModel(IOutputModel source)
        {
            SectionCode = source.SectionCode;
            SectionName = source.SectionName;
        }

    }

}
