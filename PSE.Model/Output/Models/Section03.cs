using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class KeyInformation : IKeyInformation
    {

        public string ClientName { get; set; }

        public string ClientNumber { get; set; }

        public string Portfolio { get; set; }

        public string Service { get; set; }

        public string RiskProfile { get; set; }

        public string PercentWeightedPerformance { get; set; }

        public KeyInformation() 
        { 
            ClientName = string.Empty;
            ClientNumber = string.Empty;
            Portfolio = string.Empty;
            Service = string.Empty;
            RiskProfile = string.Empty;
            PercentWeightedPerformance = string.Empty;
        }

        public KeyInformation(IKeyInformation source)
        {
            ClientName = source.ClientName;
            ClientNumber = source.ClientNumber;
            Portfolio = source.Portfolio;
            Service = source.Service;
            RiskProfile = source.RiskProfile;
            PercentWeightedPerformance = source.PercentWeightedPerformance;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class AssetType : IAssetType
    {

        public string Type { get; set; }

        public string MarketValueReportingCurrency { get; set; }

        public AssetType()
        {
            Type = string.Empty;    
            MarketValueReportingCurrency = string.Empty;
        }

        public AssetType(IAssetType source)
        {
            Type = source.Type;
            MarketValueReportingCurrency = source.MarketValueReportingCurrency;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class AssetExtract : IAssetExtract
    {

        public string AssetClass { get; set; }

        public string MarketValueReportingCurrency { get; set; }

        [JsonIgnore]
        public IList<IAssetType> AssetsType { get; set; }

        [JsonProperty("assetsType")]
        public IList<IAssetType>? SerializationAssetsType
        {
            get { return AssetsType != null && AssetsType.Any() ? (List<IAssetType>)AssetsType : null; }
            private set { }
        }

        public AssetExtract()
        {
            AssetClass = string.Empty;  
            MarketValueReportingCurrency = string.Empty;
            AssetsType = new List<IAssetType>();
        }

        public AssetExtract(IAssetExtract source)
        {
            AssetClass = source.AssetClass;
            MarketValueReportingCurrency = source.MarketValueReportingCurrency;
            AssetsType = new List<IAssetType>();
            if(source.AssetsType != null && source.AssetsType.Any()) 
            { 
                foreach(IAssetType _assetType in source.AssetsType)
                {
                    AssetsType.Add(new AssetType(_assetType));
                }
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section3Content : ISection3Content
    {

        [JsonProperty(propertyName: "keyInformation", Order = 1)]
        public IList<IKeyInformation> KeysInformation { get; set; }

        [JsonProperty(propertyName: "assetsExtract", Order = 2)]
        public IList<IAssetExtract> AssetsExtract { get; set; }

        public Section3Content()
        {
            KeysInformation = new List<IKeyInformation>();
            AssetsExtract = new List<IAssetExtract>();
        }

        public Section3Content(ISection3Content source)
        {
            KeysInformation = new List<IKeyInformation>();
            AssetsExtract = new List<IAssetExtract>();
            if (source != null)
            {
                if (source.KeysInformation != null && source.KeysInformation.Any())
                {
                    foreach (IKeyInformation _keyInformation in source.KeysInformation)
                    {
                        KeysInformation.Add(new KeyInformation(_keyInformation));
                    }
                }
                if (source.AssetsExtract != null && source.AssetsExtract.Any())
                {
                    foreach (IAssetExtract _assetExtract in source.AssetsExtract)
                    {
                        AssetsExtract.Add(new AssetExtract(_assetExtract));
                    }
                }
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section3 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection3Content Content { get; set; }

        public Section3() : base(OUTPUT_SECTION3_CODE)
        { 
            Content = new Section3Content();
        }

        public Section3(Section3 source) : base(source) 
        {
            if (source.Content != null)
                Content = new Section3Content(source.Content);
            else
                Content = new Section3Content();
        }

    }

}
