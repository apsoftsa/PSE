using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Asset : IAsset
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? AssetClass { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? MarketValueReportingCurrencyT { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PercentInvestmentT { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? TypeInvestment { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? MarketValueReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PercentInvestment { get; set; } 

        public Asset()
        {
            AssetClass = null;
            MarketValueReportingCurrencyT = null;
            PercentInvestmentT = null;
            TypeInvestment = null;
            MarketValueReportingCurrency = null;
            PercentInvestment = null;
        }

        public Asset(IAsset source)
        {
            AssetClass = source.AssetClass;
            MarketValueReportingCurrencyT = source.MarketValueReportingCurrencyT;
            PercentInvestmentT = source.PercentInvestmentT;
            TypeInvestment = source.TypeInvestment;
            MarketValueReportingCurrency = source.MarketValueReportingCurrency;
            PercentInvestment = source.PercentInvestment;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ChartAsset : IChartAsset
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? AssetClass { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PercentInvestment { get; set; } 

        public ChartAsset()
        {
            this.AssetClass = string.Empty;
            this.PercentInvestment = 0;
        }

        public ChartAsset(IChartAsset source)
        {
            this.AssetClass = source.AssetClass;
            this.PercentInvestment = source.PercentInvestment;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section6Content : ISection6Content
    {

        [JsonProperty(propertyName: "assets", Order = 1)]
        public IList<IAsset> Assets { get; set; }

        [JsonProperty(propertyName: "chartAssets", Order = 2)]
        public IList<IChartAsset> ChartAssets { get; set; }

        public Section6Content()
        {
            Assets = new List<IAsset>();    
            ChartAssets = new List<IChartAsset>();  
        }

        public Section6Content(ISection6Content source)
        {
            Assets = new List<IAsset>();
            ChartAssets = new List<IChartAsset>();
            if (source != null)
            {
                if (source.Assets != null && source.Assets.Any())
                {
                    foreach (IAsset asset in source.Assets)
                    {
                        Assets.Add(new Asset(asset));
                    }
                }
                if (source.ChartAssets != null && source.ChartAssets.Any())
                {
                    foreach (IChartAsset chartAsset in source.ChartAssets)
                    {
                        ChartAssets.Add(new ChartAsset(chartAsset));
                    }
                }
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section6 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection6Content Content { get; set; }

        public Section6() : base()
        {
            Content = new Section6Content();
        }

        public Section6(Section6 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section6Content(source.Content);
            else
                Content = new Section6Content();
        }

    }

}
