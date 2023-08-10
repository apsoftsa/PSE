using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Output.Models
{    

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Investment : IInvestment
    {

        [JsonIgnore]
        public bool IsTotal { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Currency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Amount { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Exchange { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? MarketValueReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PercentAsset { get; set; }        
        
        public Investment()
        {
            this.IsTotal = false;
            this.Amount = null;
            this.Currency = null;
            this.MarketValueReportingCurrency = null;
            this.Exchange = null;
            this.PercentAsset = null;
        }

        public Investment(IInvestment source)
        {
            this.IsTotal = source.IsTotal;
            this.Amount = source.Amount;
            this.Currency = source.Currency;
            this.MarketValueReportingCurrency = source.MarketValueReportingCurrency;
            this.Exchange = source.Exchange;
            this.PercentAsset = source.PercentAsset;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ChartInvestment : IChartInvestment
    {

        [JsonIgnore]
        public string PercentType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Currency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? PercentAsset { get; set; }

        public ChartInvestment()
        {
            this.Currency = null;
            this.PercentType = string.Empty;
            this.PercentAsset = null;
        }

        public ChartInvestment(IChartInvestment source)
        {
            this.Currency = source.Currency;
            this.PercentType = source.PercentType;
            this.PercentAsset = source.PercentAsset;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section7Content : ISection7Content
    {

        [JsonProperty(propertyName: "investment", Order = 1)]
        public IList<IInvestment> Investments { get; set; }

        [JsonProperty(propertyName: "chartInvestment", Order = 2)]
        public IList<IChartInvestment> ChartInvestments { get; set; }

        public Section7Content()
        {
            Investments = new List<IInvestment>();
            ChartInvestments = new List<IChartInvestment>();
        }

        public Section7Content(ISection7Content source)
        {
            Investments = new List<IInvestment>();
            ChartInvestments = new List<IChartInvestment>();
            if (source != null)
            {
                if (source.Investments != null && source.Investments.Any())
                {
                    foreach (IInvestment _inv in source.Investments)
                    {
                        Investments.Add(new Investment(_inv));
                    }
                }
                if (source.ChartInvestments != null && source.ChartInvestments.Any())
                {
                    foreach (IChartInvestment _chartInv in source.ChartInvestments)
                    {
                        ChartInvestments.Add(new ChartInvestment(_chartInv));
                    }
                }
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section7 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection7Content Content { get; set; }

        public Section7() : base()
        {
            Content = new Section7Content();
        }

        public Section7(Section7 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section7Content(source.Content);
            else
                Content = new Section7Content();
        }

    }

}
