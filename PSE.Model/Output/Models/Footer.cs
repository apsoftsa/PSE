using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class FooterContent : OutputModel, IFooterContent, IOutputModel
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BankAddress { get; set; }

        public FooterContent()
        {
            this.BankAddress = string.Empty;
        }

        public FooterContent(IFooterContent source)
        {
            this.BankAddress = source.BankAddress;
        }

    }

}
