using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ShareDetail : IShareDetail
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

        public ShareDetail()
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

        public ShareDetail(IShareDetail source)
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

    public class ShareSubSection : IShareSubSection
    {

        public string Name { get; set; }

        public IList<IShareDetail> Content { get; set; }

        public ShareSubSection(string name)
        {
            Name = name;
            Content = new List<IShareDetail>();
        }

        public ShareSubSection(IShareSubSection source)
        {
            Name = source.Name;
            Content = new List<IShareDetail>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new ShareDetail(item));
            }
        }

    }

    public class Section090Content : ISection090Content
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IShareSubSection Subsection9010 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IShareSubSection Subsection9020 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IShareSubSection SubSection9030 { get; set; }

        public Section090Content()
        {
            Subsection9010 = new ShareSubSection("Shares");
            Subsection9020 = new ShareSubSection("Equity Funds");
            SubSection9030 = new ShareSubSection("Derivative products on securities");
        }

        public Section090Content(ISection090Content source)
        {
            Subsection9010 = new ShareSubSection(source.Subsection9010);
            Subsection9020 = new ShareSubSection(source.Subsection9020);
            SubSection9030 = new ShareSubSection(source.SubSection9030);
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section090 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection090Content Content { get; set; }

        public Section090() : base()
        {
            Content = new Section090Content();
        }

        public Section090(Section090 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section090Content(source.Content);
            else
                Content = new Section090Content();
        }

    }

}
