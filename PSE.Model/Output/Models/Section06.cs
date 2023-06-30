using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class SubAsset : ISubAsset
    {

        public string TypeInvestment { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MarketValueReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PercentInvestment { get; set; }

        public SubAsset() 
        { 
            TypeInvestment = string.Empty;
            MarketValueReportingCurrency = string.Empty;    
            PercentInvestment = string.Empty;
        }

        public SubAsset(ISubAsset source)
        {
            TypeInvestment = source.TypeInvestment;
            MarketValueReportingCurrency = source.MarketValueReportingCurrency;
            PercentInvestment = source.PercentInvestment;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Asset : IAsset
    {

        public string AssetClass { get; set; }  

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MarketValueReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PercentInvestment { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<ISubAsset> SubAssets { get; set; }

        public Asset()
        {
            AssetClass = string.Empty;
            MarketValueReportingCurrency = string.Empty;
            PercentInvestment = string.Empty;
            SubAssets = new List<ISubAsset>();
        }

        public Asset(IAsset source)
        {
            AssetClass = source.AssetClass;
            MarketValueReportingCurrency = source.MarketValueReportingCurrency;
            PercentInvestment = source.PercentInvestment;
            SubAssets = new List<ISubAsset>();
            if (source != null && source.SubAssets != null && source.SubAssets.Any())
            {
                foreach (ISubAsset _subAss in source.SubAssets)
                {
                    SubAssets.Add(new SubAsset(_subAss));
                }
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ChartAsset : IChartAsset
    {

        public string AssetClass { get; set; }

        public string PercentInvestment { get; set; }

        public ChartAsset()
        {
            this.AssetClass = string.Empty;
            this.PercentInvestment = string.Empty;
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
                    foreach (IAsset _asset in source.Assets)
                    {
                        Assets.Add(new Asset(_asset));
                    }
                }
                if (source.ChartAssets != null && source.ChartAssets.Any())
                {
                    foreach (IChartAsset _chartAsset in source.ChartAssets)
                    {
                        ChartAssets.Add(new ChartAsset(_chartAsset));
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

        public Section6() : base(OUTPUT_SECTION6_CODE)
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
