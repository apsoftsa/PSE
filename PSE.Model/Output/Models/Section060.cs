using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;

namespace PSE.Model.Output.Models
{    

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class InvestmentCurrency : IInvestmentCurrency
    {

        public string Currency { get; set; }

        public decimal? MarketValueReportingCurrency { get; set; }

        public decimal? PercentAsset { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Amount { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Exchange { get; set; }

        public string Class {  get; set; }  
        
        public InvestmentCurrency()
        {
            this.Amount = null;
            this.Currency = string.Empty;
            this.MarketValueReportingCurrency = 0;
            this.Exchange = null;
            this.PercentAsset = 0;
            this.Class = string.Empty;  
        }

        public InvestmentCurrency(IInvestmentCurrency source)
        {
            this.Amount = source.Amount;
            this.Currency = source.Currency;
            this.MarketValueReportingCurrency = source.MarketValueReportingCurrency;
            this.Exchange = source.Exchange;
            this.PercentAsset = source.PercentAsset;
            this.Class = source.Class;  
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ChartInvestmentCurrency : IChartInvestmentCurrency
    {
       
        public string Currency { get; set; }

        public decimal? PercentAsset { get; set; }

        public ChartInvestmentCurrency()
        {
            this.Currency = string.Empty;            
            this.PercentAsset = 0;
        }

        public ChartInvestmentCurrency(IChartInvestmentCurrency source)
        {
            this.Currency = source.Currency;            
            this.PercentAsset = source.PercentAsset;
        }

    }

    public class SubSection6000Content : ISubSection6000Content
    {

        public string Name { get; set; }

        public IList<IInvestmentCurrency> Content { get; set; }

        public SubSection6000Content()
        {
            Name = "Investments";
            Content = new List<IInvestmentCurrency>();
        }

        public SubSection6000Content(ISubSection6000Content source)
        {
            Name = source.Name;
            Content = new List<IInvestmentCurrency>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new InvestmentCurrency(item));
            }
        }

    }

    public class SubSection6010Content : ISubSection6010Content
    {

        public string Name { get; set; }

        public IList<IChartInvestmentCurrency> Content { get; set; }

        public SubSection6010Content()
        {
            Name = "Chart by currency";
            Content = new List<IChartInvestmentCurrency>();
        }

        public SubSection6010Content(ISubSection6010Content source)
        {
            Name = source.Name;
            Content = new List<IChartInvestmentCurrency>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new ChartInvestmentCurrency(item));
            }
        }

    }


    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section060Content : ISection060Content
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection6000Content? SubSection6000 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection6010Content? SubSection6010 { get; set; }

        public Section060Content()
        {
            SubSection6000 = null;
            SubSection6010 = null;
        }

        public Section060Content(ISection060Content source)
        {
            SubSection6000 = (source.SubSection6000 != null) ? new SubSection6000Content(source.SubSection6000) : null;
            SubSection6010 = (source.SubSection6010 != null) ? new SubSection6010Content(source.SubSection6010) : null;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section060 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection060Content Content { get; set; }

        public Section060() : base()
        {
            Content = new Section060Content();
        }

        public Section060(Section060 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section060Content(source.Content);
            else
                Content = new Section060Content();
        }

    }

}
