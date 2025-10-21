using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class KeyInformation : IKeyInformation
    {

        public string Customer { get; set; }

        public string CustomerID { get; set; }

        public string Portfolio { get; set; }

        public int? RiskProfile { get; set; }

        public string EsgProfile { get; set; }       

        public KeyInformation() 
        { 
            Customer = string.Empty;
            CustomerID = string.Empty;
            Portfolio = string.Empty;
            EsgProfile = string.Empty;
            RiskProfile = 0;
        }

        public KeyInformation(IKeyInformation source)
        {
            CustomerID = source.CustomerID;
            Customer = source.Customer;
            Portfolio = source.Portfolio;
            EsgProfile = source.EsgProfile;
            RiskProfile = source.RiskProfile;
        }

    }

    public class SubSection1000Content : ISubSection1000Content
    {

        public string Name { get; set; }

        public IList<IKeyInformation> Content { get; set; }

        public SubSection1000Content()
        {
            Name = "Key Information";
            Content = new List<IKeyInformation>(); 
        }

        public SubSection1000Content(ISubSection1000Content source)
        {
            Name = source.Name;
            Content = new List<IKeyInformation>();
            if (source.Content != null && source.Content.Any())
            {
                foreach(var item in source.Content)
                    Content.Add(new KeyInformation(item));
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class SubSection1010Content : ISubSection1010Content
    {

        public string Name { get; set; }
        public string ManagementReportFromDate { get; set; }
        public string ManagementReportToDate { get; set; }
        public string PortfolioValueDate { get; set; }
        public decimal? PortfolioValueReportingCurrency { get; set; }
        public decimal? ContributionsValueReportingCurrency { get; set; }
        public decimal? WithdrawalsValueReportingCurrency { get; set; }
        public decimal? PortfolioValueRectifiedReportingCurrency { get; set; }
        public string PortfolioValueDate2 { get; set; }
        public decimal? PortfolioValueReportingCurrency2 { get; set; }
        public decimal? PluslessValueReportingCurrency { get; set; }
        public decimal? PercentWightedPerformance { get; set; }

        public SubSection1010Content()
        {
            Name = string.Empty;
            ManagementReportFromDate = string.Empty;
            ManagementReportToDate = string.Empty;
            PortfolioValueDate = string.Empty;
            PortfolioValueReportingCurrency = null;
            ContributionsValueReportingCurrency = null;
            WithdrawalsValueReportingCurrency = null;
            PortfolioValueRectifiedReportingCurrency = null;
            PortfolioValueDate2 = string.Empty;
            PortfolioValueReportingCurrency2 = null;
            PluslessValueReportingCurrency = null;
            PercentWightedPerformance = null;   
        }

        public SubSection1010Content(ISubSection1010Content source)
        {
            Name = source.Name;
            ManagementReportFromDate = source.ManagementReportFromDate;
            ManagementReportToDate = source.ManagementReportToDate;
            PortfolioValueDate = source.PortfolioValueDate;
            PortfolioValueReportingCurrency = source.PortfolioValueReportingCurrency;
            ContributionsValueReportingCurrency = source.ContributionsValueReportingCurrency;
            WithdrawalsValueReportingCurrency = source.WithdrawalsValueReportingCurrency;
            PortfolioValueRectifiedReportingCurrency = source.PortfolioValueRectifiedReportingCurrency;
            PortfolioValueDate2 = source.PortfolioValueDate2;
            PortfolioValueReportingCurrency2 = source.PortfolioValueReportingCurrency2;
            PluslessValueReportingCurrency = source.PluslessValueReportingCurrency;
            PercentWightedPerformance = source.PercentWightedPerformance;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class SubSection1011Content : ISubSection1011Content
    {

        public string Name { get; set; }
        public decimal? DividendAndInterestValueReportingCurrency { get; set; }
        public decimal? RealizedGainLossesValueReportingCurrency { get; set; }
        public decimal? OngoingRealizedGainLossesValueReportingCurrency { get; set; }
        public decimal? OnCurrencyRealizedGainLossesValueReportingCurrency { get; set; }
        public decimal? NotRealizedGainLossesValueReportingCurrency { get; set; }
        public decimal? OngoingNotRealizedGainLossesValueReportingCurrency { get; set; }
        public decimal? OnCurrencyNotRealizedGainLossesValueReportingCurrency { get; set; }
        public decimal? PlusLessValueReportingCurrency { get; set; }

        public SubSection1011Content()
        {
            Name = "";
            DividendAndInterestValueReportingCurrency = null;
            RealizedGainLossesValueReportingCurrency = null;
            OngoingRealizedGainLossesValueReportingCurrency = null;
            OnCurrencyRealizedGainLossesValueReportingCurrency = null;
            NotRealizedGainLossesValueReportingCurrency = null;
            OngoingNotRealizedGainLossesValueReportingCurrency = null;
            OnCurrencyNotRealizedGainLossesValueReportingCurrency = null;
            PlusLessValueReportingCurrency = null;
        }

        public SubSection1011Content(ISubSection1011Content source)
        {
            Name = source.Name;
            DividendAndInterestValueReportingCurrency = source.DividendAndInterestValueReportingCurrency;
            RealizedGainLossesValueReportingCurrency = source.RealizedGainLossesValueReportingCurrency;
            OngoingRealizedGainLossesValueReportingCurrency = source.OngoingRealizedGainLossesValueReportingCurrency;
            OnCurrencyRealizedGainLossesValueReportingCurrency = source.OnCurrencyRealizedGainLossesValueReportingCurrency;
            NotRealizedGainLossesValueReportingCurrency = source.NotRealizedGainLossesValueReportingCurrency;
            OngoingNotRealizedGainLossesValueReportingCurrency = source.OngoingNotRealizedGainLossesValueReportingCurrency;
            OnCurrencyNotRealizedGainLossesValueReportingCurrency = source.OnCurrencyNotRealizedGainLossesValueReportingCurrency;
            PlusLessValueReportingCurrency = source.PlusLessValueReportingCurrency;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section010Content : ISection010Content
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection1000Content? SubSection1000 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection1010Content? SubSection1010 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection1011Content? SubSection1011 { get; set; }

        public Section010Content()
        {
            SubSection1000 = null;   
            SubSection1010 = null;
            SubSection1011 = null;
        }

        public Section010Content(ISection010Content source)
        {
            SubSection1000 = (source.SubSection1000 != null) ? new SubSection1000Content(source.SubSection1000) : null;
            SubSection1010 = (source.SubSection1010 != null) ? new SubSection1010Content(source.SubSection1010) : null;
            SubSection1011 = (source.SubSection1011 != null) ? new SubSection1011Content(source.SubSection1011) : null;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section010 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection010Content Content { get; set; }

        public Section010() : base()
        { 
            Content = new Section010Content();
        }

        public Section010(Section010 source) : base(source) 
        {
            if (source.Content != null)
                Content = new Section010Content(source.Content);
            else
                Content = new Section010Content();
        }

    }

}
