using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class BondDetail : IBondDetail
    {

        public string Description1 { get; set; }

        public string Description2 { get; set; }

        public string Description3 { get; set; }

        public decimal? NominalAmount { get; set; }

        public string Currency { get; set; }

        public string SpRating { get; set; }

        public decimal? PercentRate { get; set; }

        public string Coupon { get; set; }

        public decimal? Duration { get; set; }

        public IList<ISummaryTo> SummaryTo { get; set; }

        public IList<ISummaryBeginningYear> SummaryBeginningYear { get; set; }

        public IList<ISummaryPurchase> SummaryPurchase { get; set; }

        public decimal? CapitalMarketValueReportingCurrency { get; set; }

        public decimal? InterestMarketValueReportingCurrency { get; set; }

        public decimal? TotalMarketValueReportingCurrency { get; set; }

        public decimal? PercentYTD { get; set; }

        public decimal? PercentWeight { get; set; }
       
        public BondDetail()
        {
            Description1 = string.Empty;
            Description2 = string.Empty;
            Description3 = string.Empty;
            NominalAmount = 0;
            Currency = string.Empty;
            SpRating = string.Empty;
            PercentRate = 0;
            Coupon = string.Empty;
            Duration = 0;
            SummaryTo = new List<ISummaryTo>();
            SummaryBeginningYear = new List<ISummaryBeginningYear>();
            SummaryPurchase = new List<ISummaryPurchase>();
            CapitalMarketValueReportingCurrency = 0;
            InterestMarketValueReportingCurrency = 0;
            TotalMarketValueReportingCurrency = 0;
            PercentYTD = 0;
            PercentWeight = 0;
        }

        public BondDetail(IBondDetail source)
        {
            Description1 = source.Description1;
            Description2 = source.Description2;
            Description3 = source.Description3;
            NominalAmount = source.NominalAmount;
            Currency = source.Currency;
            SpRating = source.SpRating;
            PercentRate = source.PercentRate;
            Coupon = source.Coupon;
            Duration = source.Duration;
            SummaryTo = new List<ISummaryTo>();
            if (source.SummaryTo != null && source.SummaryTo.Any())
            {
                foreach (var item in source.SummaryTo)
                    SummaryTo.Add(new SummaryTo(item));
            }
            SummaryBeginningYear = new List<ISummaryBeginningYear>();
            if (source.SummaryBeginningYear != null && source.SummaryBeginningYear.Any())
            {
                foreach (var item in source.SummaryBeginningYear)
                    SummaryBeginningYear.Add(new SummaryBeginningYear(item));
            }
            SummaryPurchase = new List<ISummaryPurchase>();
            if (source.SummaryPurchase != null && source.SummaryPurchase.Any())
            {
                foreach (var item in source.SummaryPurchase)
                    SummaryPurchase.Add(new SummaryPurchase(item));
            }
            CapitalMarketValueReportingCurrency = source.CapitalMarketValueReportingCurrency;
            InterestMarketValueReportingCurrency = source.InterestMarketValueReportingCurrency;
            TotalMarketValueReportingCurrency = source.TotalMarketValueReportingCurrency;
            PercentYTD = source.PercentYTD;
            PercentWeight = source.PercentWeight;
        }
    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class BondFundDetail : IBondFundDetail
    {

        public string Description1 { get; set; }

        public string Description2 { get; set; }

        public string Description3 { get; set; }

        public decimal? Quantity { get; set; }

        public string Currency { get; set; }

        public IList<ISummaryTo> SummaryTo { get; set; }

        public IList<ISummaryBeginningYear> SummaryBeginningYear { get; set; }

        public IList<ISummaryPurchase> SummaryPurchase { get; set; }

        public decimal? CapitalMarketValueReportingCurrency { get; set; }

        public decimal? TotalMarketValueReportingCurrency { get; set; }

        public decimal? PercentWeight { get; set; }

        public BondFundDetail()
        {
            Description1 = string.Empty;
            Description2 = string.Empty;
            Description3 = string.Empty;
            Quantity = 0;
            Currency = string.Empty;
            SummaryTo = new List<ISummaryTo>();
            SummaryBeginningYear = new List<ISummaryBeginningYear>();
            SummaryPurchase = new List<ISummaryPurchase>();
            CapitalMarketValueReportingCurrency = 0;
            TotalMarketValueReportingCurrency = 0;
            PercentWeight = 0;
        }

        public BondFundDetail(IBondFundDetail source)
        {
            Description1 = source.Description1;
            Description2 = source.Description2;
            Description3 = source.Description3;
            Quantity = source.Quantity;
            Currency = source.Currency;
            SummaryTo = new List<ISummaryTo>();
            if (source.SummaryTo != null && source.SummaryTo.Any())
            {
                foreach (var item in source.SummaryTo)
                    SummaryTo.Add(new SummaryTo(item));
            }
            SummaryBeginningYear = new List<ISummaryBeginningYear>();
            if (source.SummaryBeginningYear != null && source.SummaryBeginningYear.Any())
            {
                foreach (var item in source.SummaryBeginningYear)
                    SummaryBeginningYear.Add(new SummaryBeginningYear(item));
            }
            SummaryPurchase = new List<ISummaryPurchase>();
            if (source.SummaryPurchase != null && source.SummaryPurchase.Any())
            {
                foreach (var item in source.SummaryPurchase)
                    SummaryPurchase.Add(new SummaryPurchase(item));
            }
            CapitalMarketValueReportingCurrency = source.CapitalMarketValueReportingCurrency;
            TotalMarketValueReportingCurrency = source.TotalMarketValueReportingCurrency;
            PercentWeight = source.PercentWeight;
        }
    }

    public class BondSubSection : IBondSubSection
    {

        public string Name { get; set; }

        public IList<IBondDetail> Content { get; set; }

        public BondSubSection(string name)
        {
            Name = name;
            Content = new List<IBondDetail>();
        }

        public BondSubSection(IBondSubSection source)
        {
            Name = source.Name;
            Content = new List<IBondDetail>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new BondDetail(item));
            }
        }

    }

    public class FundSubSection : IFundSubSection
    {

        public string Name { get; set; }

        public IList<IBondFundDetail> Content { get; set; }

        public FundSubSection(string name)
        {
            Name = name;
            Content = new List<IBondFundDetail>();
        }

        public FundSubSection(IFundSubSection source)
        {
            Name = source.Name;
            Content = new List<IBondFundDetail>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content.OrderByDescending(ob => ob.CapitalMarketValueReportingCurrency))
                    Content.Add(new BondFundDetail(item));
            }
        }

    }

    public class Section080Content : ISection080Content
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IBondSubSection? SubSection8000 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IBondSubSection? SubSection8010 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IBondSubSection? SubSection8020 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IBondSubSection? SubSection8030 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IFundSubSection? SubSection8040 { get; set; }

        public Section080Content()
        {            
            SubSection8000 = null;
            SubSection8010 = null;
            SubSection8020 = null;
            SubSection8030 = null;
            SubSection8040 = null;
        }

        public Section080Content(ISection080Content source)
        {
            SubSection8000 = (source.SubSection8000 != null) ? new BondSubSection(source.SubSection8000) : null;
            SubSection8010 = (source.SubSection8010 != null) ? new BondSubSection(source.SubSection8010) : null;
            SubSection8020 = (source.SubSection8020 != null) ? new BondSubSection(source.SubSection8020) : null;
            SubSection8030 = (source.SubSection8030 != null) ? new BondSubSection(source.SubSection8030) : null;
            SubSection8040 = (source.SubSection8040 != null) ? new FundSubSection(source.SubSection8040) : null;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section080 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection080Content Content { get; set; }

        public Section080() : base()
        {
            Content = new Section080Content();
        }

        public Section080(Section080 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section080Content(source.Content);
            else
                Content = new Section080Content();
        }

    }

}
