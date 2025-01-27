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

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Logo { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? CompanyName { get; set; }

        public string RequestUUID { get; set; }

        public string ReferenceString1 { get; set; }

        public string ReferenceString2 { get; set; }

        public string ReferenceString3 { get; set; }

        public string ReferenceString4 { get; set; }

        public string ReferenceString5 { get; set; }

        public string ReferenceString6 { get; set; }

        public string ReferenceString7 { get; set; }

        public string ReferenceString8 { get; set; }

        public string ReferenceString9 { get; set; }

        public string ReferenceString10 { get; set; }

        //public IList<IHeaderDescription> Description { get; set; }

        public HeaderContent()
        {
            this.Logo = null;
            this.CompanyName = null;
            this.RequestUUID = string.Empty;
            this.ReferenceString1 = string.Empty;
            this.ReferenceString2 = string.Empty;
            this.ReferenceString3 = string.Empty;
            this.ReferenceString4 = string.Empty;
            this.ReferenceString5 = string.Empty;
            this.ReferenceString6 = string.Empty;
            this.ReferenceString7 = string.Empty;
            this.ReferenceString8 = string.Empty;
            this.ReferenceString9 = string.Empty;
            this.ReferenceString10 = string.Empty;
            //this.Description = new List<IHeaderDescription>() { new HeaderDescription() };
        }

        public HeaderContent(IHeaderContent source)
        {
            this.Logo = source.Logo;
            this.CompanyName = source.CompanyName;
            this.RequestUUID = source.RequestUUID;
            this.ReferenceString1 = source.ReferenceString1;
            this.ReferenceString2 = source.ReferenceString2;
            this.ReferenceString3 = source.ReferenceString3;
            this.ReferenceString4 = source.ReferenceString4;
            this.ReferenceString5 = source.ReferenceString5;
            this.ReferenceString6 = source.ReferenceString6;
            this.ReferenceString7 = source.ReferenceString7;
            this.ReferenceString8 = source.ReferenceString8;
            this.ReferenceString9 = source.ReferenceString9;
            this.ReferenceString10 = source.ReferenceString10;
            /*
            this.Description = new List<IHeaderDescription>();
            if (source.Description != null && source.Description.Any())
            {                
                foreach (var item in source.Description)
                    this.Description.Add(new HeaderDescription(item));
            }
            */
        }

    }
   
}
