﻿using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ShareDetail : IShareDetail
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description1 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description2 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal Amount { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Currency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<ISummaryTo> SummaryTo { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<ISummaryBeginningYear> SummaryBeginningYear { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<ISummaryPurchase> SummaryPurchase { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal CapitalMarketValueReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal PercentWeight { get; set; }

        public ShareDetail()
        {
            Description1 = string.Empty;
            Description2 = string.Empty;            
            Amount = 0;
            Currency = string.Empty;
            SummaryTo = new List<ISummaryTo>();
            SummaryBeginningYear = new List<ISummaryBeginningYear>();
            SummaryPurchase = new List<ISummaryPurchase>();
            CapitalMarketValueReportingCurrency = 0;
            PercentWeight = 0;
        }

        public ShareDetail(IShareDetail source)
        {
            Description1 = source.Description1;
            Description2 = source.Description2;            
            Amount = source.Amount;
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
            PercentWeight = source.PercentWeight;
        }
    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class EquityFundDetail : ShareDetail, IEquityFundDetail
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description3 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal TotalMarketValueReportingCurrency { get; set; }

        public EquityFundDetail() : base()
        {
            Description3 = string.Empty;
            TotalMarketValueReportingCurrency = 0;
        }

        public EquityFundDetail(IEquityFundDetail source) : base(source)
        {
            Description3 = source.Description3;
            TotalMarketValueReportingCurrency = source.TotalMarketValueReportingCurrency;
        }
    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class DerivateDetail : EquityFundDetail, IDerivateDetail
    {

        public DerivateDetail() : base() { }

        public DerivateDetail(IDerivateDetail source) : base(source) { }

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

    public class EquityFundSubSection : IEquityFundSubSection
    {

        public string Name { get; set; }

        public IList<IEquityFundDetail> Content { get; set; }

        public EquityFundSubSection(string name)
        {
            Name = name;
            Content = new List<IEquityFundDetail>();
        }

        public EquityFundSubSection(IEquityFundSubSection source)
        {
            Name = source.Name;
            Content = new List<IEquityFundDetail>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new EquityFundDetail(item));
            }
        }

    }

    public class DerivateSubSection : IDerivateSubSection
    {

        public string Name { get; set; }

        public IList<IDerivateDetail> Content { get; set; }

        public DerivateSubSection(string name)
        {
            Name = name;
            Content = new List<IDerivateDetail>();
        }

        public DerivateSubSection(IDerivateSubSection source)
        {
            Name = source.Name;
            Content = new List<IDerivateDetail>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new DerivateDetail(item));
            }
        }

    }

    public class Section090Content : ISection090Content
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IShareSubSection Subsection9010 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEquityFundSubSection Subsection9020 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IDerivateSubSection SubSection9030 { get; set; }

        public Section090Content()
        {
            Subsection9010 = new ShareSubSection("Shares");
            Subsection9020 = new EquityFundSubSection("Equity Funds");
            SubSection9030 = new DerivateSubSection("Derivative products on securities");
        }

        public Section090Content(ISection090Content source)
        {
            Subsection9010 = new ShareSubSection(source.Subsection9010);
            Subsection9020 = new EquityFundSubSection(source.Subsection9020);
            SubSection9030 = new DerivateSubSection(source.SubSection9030);
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
