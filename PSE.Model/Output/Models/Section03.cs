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

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? CustomerName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? CustomerNumber { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Portfolio { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Service { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? RiskProfile { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PercentWeightedPerformance { get; set; }

        public KeyInformation() 
        { 
            CustomerName = null;
            CustomerNumber = null;
            Portfolio = null;
            Service = null;
            RiskProfile = null;
            PercentWeightedPerformance = null;
        }

        public KeyInformation(IKeyInformation source)
        {
            CustomerNumber = source.CustomerNumber;
            CustomerName = source.CustomerName;
            Portfolio = source.Portfolio;
            Service = source.Service;
            RiskProfile = source.RiskProfile;
            PercentWeightedPerformance = source.PercentWeightedPerformance;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class AssetExtract : IAssetExtract
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? AssetClass { get; set; }

        [JsonProperty(propertyName: "type", NullValueHandling = NullValueHandling.Ignore)]
        public string? AssetType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? MarketValueReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? MarketValueReportingCurrencyT { get; set; }

        public AssetExtract()
        {
            AssetClass = null;  
            AssetType = null;   
            MarketValueReportingCurrency = null;
            MarketValueReportingCurrencyT = null;
        }

        public AssetExtract(IAssetExtract source)
        {
            AssetClass = source.AssetClass;
            AssetType = source.AssetType;
            MarketValueReportingCurrency = source.MarketValueReportingCurrency;
            MarketValueReportingCurrencyT = source.MarketValueReportingCurrencyT;
        }

    }

    public class FooterInformation : IFooterInformation
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Footer1 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Footer2 { get; set; }

        public FooterInformation()
        {
            Footer1 = null;
            Footer2 = null;
        }

        public FooterInformation(IFooterInformation source)
        {
            Footer1 = source.Footer1;
            Footer2 = source.Footer2;
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

        [JsonProperty(propertyName: "dividendsInterests", Order = 3)]
        public IList<IAssetExtract> DividendsInterests { get; set; }

        [JsonProperty(propertyName: "footerInformation", Order = 4)]
        public IList<IFooterInformation> FooterInformation { get; set; }

        public Section3Content()
        {
            KeysInformation = new List<IKeyInformation>();
            AssetsExtract = new List<IAssetExtract>();
            DividendsInterests = new List<IAssetExtract>();
            FooterInformation = new List<IFooterInformation>();
        }

        public Section3Content(ISection3Content source)
        {
            KeysInformation = new List<IKeyInformation>();
            AssetsExtract = new List<IAssetExtract>();
            DividendsInterests = new List<IAssetExtract>();
            FooterInformation = new List<IFooterInformation>();
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
                if (source.DividendsInterests != null && source.DividendsInterests.Any())
                {
                    foreach (IAssetExtract _dividendInterest in source.DividendsInterests)
                    {
                        DividendsInterests.Add(new AssetExtract(_dividendInterest));
                    }
                }
                if (source.FooterInformation != null && source.FooterInformation.Any())
                {
                    foreach (IFooterInformation _footerInf in source.FooterInformation)
                    {
                        FooterInformation.Add(new FooterInformation(_footerInf));
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

        public Section3() : base()
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
