using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class HeaderDescription : IHeaderDescription
    {

        public IList<ISettled>? Settled { get; set; }

        public string Assessment { get; set; }

        public string CustomerID { get; set; }

        public HeaderDescription()
        {
            this.Settled = new List<ISettled>() { new Settled() };
            this.Assessment = string.Empty;
            this.CustomerID = string.Empty;
        }

        public HeaderDescription(IHeaderDescription source)
        {
            if (source.Settled != null && source.Settled.Any())
            {
                this.Settled = new List<ISettled>();
                foreach (var item in source.Settled)
                    this.Settled.Add(new Settled(item));
            }
            else
                this.Settled = null;
            this.Assessment = source.Assessment;
            this.CustomerID = source.CustomerID;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class HeaderContent : OutputModel, IHeaderContent, IOutputModel
    {

        public string Logo { get; set; }

        public string CompanyName { get; set; }

        public string RequestUUID { get; set; }

        public IList<IHeaderDescription> Description { get; set; }

        public HeaderContent()
        {
            this.Logo = string.Empty;
            this.CompanyName = string.Empty;
            this.RequestUUID = string.Empty;
            this.Description = new List<IHeaderDescription>() { new HeaderDescription() };
        }

        public HeaderContent(IHeaderContent source)
        {
            this.Logo = source.Logo;
            this.CompanyName = source.CompanyName;
            this.RequestUUID = source.RequestUUID;
            this.Description = new List<IHeaderDescription>();
            if (source.Description != null && source.Description.Any())
            {                
                foreach (var item in source.Description)
                    this.Description.Add(new HeaderDescription(item));
            }
        }

    }
   
}
