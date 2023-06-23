using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class HeaderContent : OutputModel, IHeaderContent, IOutputModel
    {

        public string Logo { get; set; }

        public string CompanyName { get; set; }

        public HeaderContent()
        {
            this.Logo = string.Empty;
            this.CompanyName = string.Empty;
        }

        public HeaderContent(IHeaderContent source)
        {
            this.Logo = source.Logo;
            this.CompanyName = source.CompanyName;
        }

    }
   
}
