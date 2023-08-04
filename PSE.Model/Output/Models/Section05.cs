using Newtonsoft.Json;
using PSE.Model.Common;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class MultilineManagement : IMultilineManagement
    {

        [JsonIgnore]
        public bool IsTotal { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? ManagementLine { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Currency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? MarketValueReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PercentWeight { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? PercentPerformance { get; set; }        

        public MultilineManagement()
        {
            this.IsTotal = false;
            this.ManagementLine = null;
            this.Currency = null;
            this.MarketValueReportingCurrency = null;
            this.PercentWeight = null;
            this.PercentPerformance = null;
        }

        public MultilineManagement(IMultilineManagement source)
        {
            this.IsTotal = source.IsTotal;
            this.ManagementLine = source.ManagementLine;
            this.Currency = source.Currency;
            this.MarketValueReportingCurrency = source.MarketValueReportingCurrency;
            this.PercentWeight = source.PercentWeight;
            this.PercentPerformance = source.PercentPerformance;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ChartMultilineManagement : IChartMultilineManagement
    {

        [JsonIgnore]
        public string? Currency { get;set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? ManagementLine { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Percent { get; set; }

        public ChartMultilineManagement()
        {
            this.Currency= null;    
            this.ManagementLine = null;
            this.Percent = null;
        }

        public ChartMultilineManagement(IChartMultilineManagement source)
        {
            this.Currency = source.Currency;
            this.ManagementLine = source.ManagementLine;
            this.Percent = source.Percent;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section5Content : ISection5Content
    {

        [JsonProperty(propertyName: "multilineManagement", Order = 1)]
        public IList<IMultilineManagement> MultilinesManagement { get; set; }

        [JsonProperty(propertyName: "chartMultilineManagement", Order = 2)]
        public IList<IChartMultilineManagement> ChartMultilinesManagement { get; set; }

        public Section5Content()
        {
            MultilinesManagement = new List<IMultilineManagement>();
            ChartMultilinesManagement = new List<IChartMultilineManagement>();
        }

        public Section5Content(ISection5Content source)
        {
            MultilinesManagement = new List<IMultilineManagement>();
            ChartMultilinesManagement = new List<IChartMultilineManagement>();
            if (source != null)
            {
                if (source.MultilinesManagement != null && source.MultilinesManagement.Any())
                {
                    foreach (IMultilineManagement _multiMan in source.MultilinesManagement)
                    {
                        MultilinesManagement.Add(new MultilineManagement(_multiMan));
                    }
                }
                if (source.ChartMultilinesManagement != null && source.ChartMultilinesManagement.Any())
                {
                    foreach (IChartMultilineManagement _chartMultiMan in source.ChartMultilinesManagement)
                    {
                        ChartMultilinesManagement.Add(new ChartMultilineManagement(_chartMultiMan));
                    }
                }
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section5 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection5Content Content { get; set; }

        public Section5() : base(OUTPUT_SECTION5_CODE)
        {
            Content = new Section5Content();
        }

        public Section5(Section5 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section5Content(source.Content);
            else
                Content = new Section5Content();
        }

    }

}
