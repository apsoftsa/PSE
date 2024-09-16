using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class BondDetail : IBondDetail
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Description1 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Description2 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Description3 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? NominalAmount { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Currency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? SpRating { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PercentRate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Coupon { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Duration { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<IDetailSummary> SummaryTo { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<IDetailSummary> SummaryBeginningYear { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<IDetailSummary> SummaryPurchase { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? CapitalMarketValueReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? InterestMarketValueReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? TotalMarketValueReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PercentYTD { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PercentWeight { get; set; }
       
        public BondDetail()
        {
            Description1 = null;
            Description2 = null;
            Description3 = null;
            NominalAmount = null;
            Currency = null;
            SpRating = null;
            PercentRate = null;
            Coupon = null;
            Duration = null;
            SummaryTo = new List<IDetailSummary>();
            SummaryBeginningYear = new List<IDetailSummary>();
            SummaryPurchase = new List<IDetailSummary>();
            CapitalMarketValueReportingCurrency = null;
            InterestMarketValueReportingCurrency = null;
            TotalMarketValueReportingCurrency = null;
            PercentYTD = null;
            PercentWeight = null;
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
            SummaryTo = new List<IDetailSummary>();
            if (source.SummaryTo != null && source.SummaryTo.Any())
            {
                foreach (var item in source.SummaryTo)
                    SummaryTo.Add(new DetailSummary(item));
            }
            SummaryBeginningYear = new List<IDetailSummary>();
            if (source.SummaryBeginningYear != null && source.SummaryBeginningYear.Any())
            {
                foreach (var item in source.SummaryBeginningYear)
                    SummaryBeginningYear.Add(new DetailSummary(item));
            }
            SummaryPurchase = new List<IDetailSummary>();
            if (source.SummaryPurchase != null && source.SummaryPurchase.Any())
            {
                foreach (var item in source.SummaryPurchase)
                    SummaryPurchase.Add(new DetailSummary(item));
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

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Description1 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Description2 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Description3 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Quantity { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Currency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<IDetailSummary> SummaryTo { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<IDetailSummary> SummaryBeginningYear { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<IDetailSummary> SummaryPurchase { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? CapitalMarketValueReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? TotalMarketValueReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PercentWeight { get; set; }

        public BondFundDetail()
        {
            Description1 = null;
            Description2 = null;
            Description3 = null;
            Quantity = null;
            Currency = null;
            SummaryTo = new List<IDetailSummary>();
            SummaryBeginningYear = new List<IDetailSummary>();
            SummaryPurchase = new List<IDetailSummary>();
            CapitalMarketValueReportingCurrency = null;
            TotalMarketValueReportingCurrency = null;
            PercentWeight = null;
        }

        public BondFundDetail(IBondFundDetail source)
        {
            Description1 = source.Description1;
            Description2 = source.Description2;
            Description3 = source.Description3;
            Quantity = source.Quantity;
            Currency = source.Currency;
            SummaryTo = new List<IDetailSummary>();
            if (source.SummaryTo != null && source.SummaryTo.Any())
            {
                foreach (var item in source.SummaryTo)
                    SummaryTo.Add(new DetailSummary(item));
            }
            SummaryBeginningYear = new List<IDetailSummary>();
            if (source.SummaryBeginningYear != null && source.SummaryBeginningYear.Any())
            {
                foreach (var item in source.SummaryBeginningYear)
                    SummaryBeginningYear.Add(new DetailSummary(item));
            }
            SummaryPurchase = new List<IDetailSummary>();
            if (source.SummaryPurchase != null && source.SummaryPurchase.Any())
            {
                foreach (var item in source.SummaryPurchase)
                    SummaryPurchase.Add(new DetailSummary(item));
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
                foreach (var item in source.Content)
                    Content.Add(new BondFundDetail(item));
            }
        }

    }

    public class Section080Content : ISection080Content
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IBondSubSection Subsection8000 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IBondSubSection Subsection8010 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IBondSubSection Subsection8020 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IBondSubSection SubSection8030 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IFundSubSection Subsection8040 { get; set; }

        public Section080Content()
        {
            Subsection8000 = new BondSubSection("Bonds with maturity <= 1 year");
            Subsection8010 = new BondSubSection("Bonds with maturity <= 5 year");
            Subsection8020 = new BondSubSection("Bonds with maturity > 5 year");
            SubSection8030 = new BondSubSection("Convertible bonds, bonds with warrants");
            Subsection8040 = new FundSubSection("Bond funds");
        }

        public Section080Content(ISection080Content source)
        {
            Subsection8000 = new BondSubSection(source.Subsection8000);
            Subsection8010 = new BondSubSection(source.Subsection8010);
            Subsection8020 = new BondSubSection(source.Subsection8020);
            SubSection8030 = new BondSubSection(source.SubSection8030);
            Subsection8040 = new FundSubSection(source.Subsection8040);
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
