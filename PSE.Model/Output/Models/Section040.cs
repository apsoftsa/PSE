using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class InvestmentAsset : IInvestmentAsset
    {

        public string AssetClass { get; set; }

        public string Class { get; set; }

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

        public InvestmentAsset()
        {
            AssetClass = string.Empty;
            Class = string.Empty;   
            MarketValueReportingCurrencyT = null;
            PercentInvestmentT = null;
            TypeInvestment = null;
            MarketValueReportingCurrency = null;
            PercentInvestment = null;
        }

        public InvestmentAsset(IInvestmentAsset source)
        {
            AssetClass = source.AssetClass;
            Class = source.Class;
            MarketValueReportingCurrencyT = source.MarketValueReportingCurrencyT;
            PercentInvestmentT = source.PercentInvestmentT;
            TypeInvestment = source.TypeInvestment;
            MarketValueReportingCurrency = source.MarketValueReportingCurrency;
            PercentInvestment = source.PercentInvestment;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class AssetClassChart : IAssetClassChart
    {

        public string AssetClass { get; set; }

        public decimal? PercentInvestment { get; set; } 

        public AssetClassChart()
        {
            this.AssetClass = string.Empty;
            this.PercentInvestment = 0;
        }

        public AssetClassChart(IAssetClassChart source)
        {
            this.AssetClass = source.AssetClass;
            this.PercentInvestment = source.PercentInvestment;
        }

    }

    public class SubSection4000Content : ISubSection4000Content
    {

        public string Name { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public bool HasMeaningfulData { get; set; }

        public int ItemsCount { get; set; }

        public IList<IInvestmentAsset> Content { get; set; }

        public SubSection4000Content()
        {
            Name = "Investments";
            HasMeaningfulData = false;
            ItemsCount = 0; 
            Content = new List<IInvestmentAsset>();
        }

        public SubSection4000Content(ISubSection4000Content source)
        {
            Name = source.Name;
            HasMeaningfulData |= source.HasMeaningfulData; 
            ItemsCount |= source.ItemsCount;    
            Content = new List<IInvestmentAsset>();
            if (source.Content != null && source.Content.Any())
            {                
                foreach (var item in source.Content)
                    Content.Add(new InvestmentAsset(item));
            }
        }

    }

    public class SubSection4010Content : ISubSection4010Content
    {

        public string Name { get; set; }

        public IList<IAssetClassChart> Content { get; set; }

        public SubSection4010Content()
        {
            Name = "Asset class chart";
            Content = new List<IAssetClassChart>();
        }

        public SubSection4010Content(ISubSection4010Content source)
        {
            Name = source.Name;
            Content = new List<IAssetClassChart>();
            if (source.Content != null && source.Content.Any())
            {                
                foreach (var item in source.Content)
                    Content.Add(new AssetClassChart(item));
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section040Content : ISection040Content
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection4000Content? SubSection4000 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection4010Content? SubSection4010 { get; set; }

        public Section040Content()
        {
            SubSection4000 = null;
            SubSection4010 = null;
        }

        public Section040Content(ISection040Content source)
        {
            SubSection4000 = (source.SubSection4000 != null) ? new SubSection4000Content(source.SubSection4000) : null;
            SubSection4010 = (source.SubSection4010 != null) ? new SubSection4010Content(source.SubSection4010) : null;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section040 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection040Content Content { get; set; }

        public Section040() : base()
        {
            Content = new Section040Content();
        }

        public Section040(Section040 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section040Content(source.Content);
            else
                Content = new Section040Content();
        }

    }

}
