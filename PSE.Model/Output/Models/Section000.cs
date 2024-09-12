﻿using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class AssetStatement : IAssetStatement
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Customer { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? CustomerID { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<ISettled>? Settled { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Portfolio { get; set; }
      
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Advisory { get; set; }

        public AssetStatement()
        {
            this.Customer = null;
            this.CustomerID = null;
            this.Settled = null;
            this.Portfolio = null;
            this.Advisory = null;
        }

        public AssetStatement(IAssetStatement source)
        {
            this.Customer = source.Customer;
            this.CustomerID = source.CustomerID;
            if (source.Settled != null && source.Settled.Any())
            {
                this.Settled = new List<ISettled>();
                foreach (var item in source.Settled)
                    this.Settled.Add(new Settled(item));
            }
            else
                this.Settled = null;
            this.Portfolio = source.Portfolio;            
            this.Advisory = source.Advisory;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section000Content : ISection000Content
    {

        [JsonProperty(propertyName: "assetStatement")]
        public IList<IAssetStatement> AssetStatements { get; set; }

        public Section000Content()
        {
            AssetStatements = new List<IAssetStatement>();
        }

        public Section000Content(ISection000Content source)
        {
            AssetStatements = new List<IAssetStatement>();
            if (source != null)
            {
                if (source.AssetStatements != null && source.AssetStatements.Any())
                {
                    foreach (IAssetStatement assetSttmnt in source.AssetStatements)
                    {
                        AssetStatements.Add(new AssetStatement(assetSttmnt));
                    }
                }
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section000 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection000Content Content { get; set; }

        public Section000() : base()
        {
            Content = new Section000Content();
        }

        public Section000(Section000 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section000Content(source.Content);
            else
                Content = new Section000Content();
        }

    }

}
