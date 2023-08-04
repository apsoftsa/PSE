using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class AssetStatement : IAssetStatement
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Customer { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Portfolio { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Date { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Advisor { get; set; }

        public AssetStatement()
        {
            this.Customer = null;   
            this.Portfolio = null;
            this.Date = null;
            this.Advisor = null;
        }

        public AssetStatement(IAssetStatement source)
        {
            this.Customer = source.Customer;
            this.Portfolio = source.Portfolio;
            this.Date = source.Date;
            this.Advisor = source.Advisor;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section1Content : ISection1Content
    {

        [JsonProperty(propertyName: "assetStatement")]
        public IList<IAssetStatement> AssetStatements { get; set; }

        public Section1Content()
        {
            AssetStatements = new List<IAssetStatement>();
        }

        public Section1Content(ISection1Content source)
        {
            AssetStatements = new List<IAssetStatement>();
            if (source != null)
            {
                if (source.AssetStatements != null && source.AssetStatements.Any())
                {
                    foreach (IAssetStatement _assetSttmnt in source.AssetStatements)
                    {
                        AssetStatements.Add(new AssetStatement(_assetSttmnt));
                    }
                }
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section1 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection1Content Content { get; set; }

        public Section1() : base(OUTPUT_SECTION1_CODE)
        {
            Content = new Section1Content();
        }

        public Section1(Section1 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section1Content(source.Content);
            else
                Content = new Section1Content();
        }

    }

}
