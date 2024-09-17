using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class InvestmentDetail : IInvestmentDetail
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Description1 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Description2 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Description3 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Amount { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Currency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<IDetailSummary>? SummaryTo { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<IDetailSummary>? SummaryBeginningYear { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<IDetailSummary>? SummaryPurchase { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? CapitalMarketValueReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? TotalMarketValueReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PercentWeight { get; set; }

        public InvestmentDetail()
        {
            Description1 = null;
            Description2 = null;
            Description3 = null;
            Amount = null;
            Currency = null;
            SummaryTo = new List<IDetailSummary>();
            SummaryBeginningYear = new List<IDetailSummary>();
            SummaryPurchase = new List<IDetailSummary>();
            CapitalMarketValueReportingCurrency = null;
            TotalMarketValueReportingCurrency = null;
            PercentWeight = null;
        }

        public InvestmentDetail(IInvestmentDetail source)
        {
            Description1 = source.Description1;
            Description2 = source.Description2;
            Description3 = source.Description3;
            Amount = source.Amount;
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

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class BondInvestmentDetail : InvestmentDetail, IBondInvestmentDetail
    {
       
        [JsonProperty( NullValueHandling = NullValueHandling.Ignore)]
        public decimal? NominalAmount { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PercentRate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Coupon { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? InterestMarketValueReportingCurrency { get; set; }

        public BondInvestmentDetail() : base()
        {
            NominalAmount = null;
            PercentRate = null;
            Coupon = null;
            InterestMarketValueReportingCurrency = null;
        }

        public BondInvestmentDetail(IBondInvestmentDetail source) : base(source)
        {
            NominalAmount = source.NominalAmount;
            PercentRate = source.PercentRate;
            Coupon = source.Coupon;
            InterestMarketValueReportingCurrency = source.InterestMarketValueReportingCurrency;
        }
    }

    public class SubSection11000 : ISubSection11000
    {
        public string Name { get; set; }
        public IList<IInvestmentDetail>? Content { get; set; }

        public SubSection11000(string name)
        {
            Name = name;
            Content = new List<IInvestmentDetail>();
        }

        public SubSection11000(ISubSection11000 source)
        {
            Name = source.Name;
            Content = new List<IInvestmentDetail>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new InvestmentDetail(item));
            }
        }
    }

    public class SubSection11010 : ISubSection11010
    {
        public string Name { get; set; }
        public IList<IBondInvestmentDetail>? Content { get; set; }

        public SubSection11010(string name)
        {
            Name = name;
            Content = new List<IBondInvestmentDetail>();
        }

        public SubSection11010(ISubSection11010 source)
        {
            Name = source.Name;
            Content = new List<IBondInvestmentDetail>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new BondInvestmentDetail(item));
            }
        }
    }

    public class SubSection11020 : ISubSection11020
    {
        public string Name { get; set; }
        public IList<IInvestmentDetail>? Content { get; set; }

        public SubSection11020(string name)
        {
            Name = name;
            Content = new List<IInvestmentDetail>();
        }

        public SubSection11020(ISubSection11020 source)
        {
            Name = source.Name;
            Content = new List<IInvestmentDetail>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new InvestmentDetail(item));
            }
        }
    }

    public class SubSection11030 : ISubSection11030
    {
        public string Name { get; set; }
        public IList<IInvestmentDetail>? Content { get; set; }

        public SubSection11030(string name)
        {
            Name = name;
            Content = new List<IInvestmentDetail>();
        }

        public SubSection11030(ISubSection11030 source)
        {
            Name = source.Name;
            Content = new List<IInvestmentDetail>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new InvestmentDetail(item));
            }
        }
    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section110Content : ISection110Content
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection11000 SubSection11000 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection11010 SubSection11010 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection11020 SubSection11020 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection11030 SubSection11030 { get; set; }

        public Section110Content()
        {
            SubSection11000 = new SubSection11000("Mix funds");
            SubSection11010 = new SubSection11010("Alternative Products – Different");
            SubSection11020 = new SubSection11020("Derivative Products – Futures");
            SubSection11030 = new SubSection11030("Real estate investments – Real estate funds");
        }

        public Section110Content(ISection110Content source)
        {
            SubSection11000 = new SubSection11000(source.SubSection11000);
            SubSection11010 = new SubSection11010(source.SubSection11010);
            SubSection11020 = new SubSection11020(source.SubSection11020);
            SubSection11030 = new SubSection11030(source.SubSection11030);
        }
    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section110 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection110Content Content { get; set; }

        public Section110() : base()
        {
            Content = new Section110Content();
        }

        public Section110(Section110 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section110Content(source.Content);
            else
                Content = new Section110Content();
        }

    }

}
