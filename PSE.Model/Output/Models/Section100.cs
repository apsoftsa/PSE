﻿using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class MetalAccount : IMetalAccount
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Account { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal CurrentBalance { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal MarketValueReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal PercentWeight { get; set; }

        public MetalAccount()
        {
            Account = string.Empty;
            CurrentBalance = 0;
            MarketValueReportingCurrency = 0;
            PercentWeight = 0;
        }

        public MetalAccount(IMetalAccount source)
        {
            Account = source.Account;
            CurrentBalance = source.CurrentBalance;
            MarketValueReportingCurrency = source.MarketValueReportingCurrency;
            PercentWeight = source.PercentWeight;
        }

    }


    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class MetalDetail : IMetalDetail
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description1 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description2 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal Quantity { get; set; }

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
        public decimal TotalMarketValueReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal PercentWeight { get; set; }

        public MetalDetail()
        {
            Description1 = string.Empty;
            Description2 = string.Empty;
            Quantity = 0;
            Currency = string.Empty;
            SummaryTo = new List<ISummaryTo>();
            SummaryBeginningYear = new List<ISummaryBeginningYear>();
            SummaryPurchase = new List<ISummaryPurchase>();
            CapitalMarketValueReportingCurrency = 0;
            TotalMarketValueReportingCurrency = 0;
            PercentWeight = 0;
        }

        public MetalDetail(IMetalDetail source)
        {
            Description1 = source.Description1;
            Description2 = source.Description2;
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

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class DerivateMetalDetail : IDerivateMetalDetail
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description1 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description2 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description3 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal Amount { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal Strike { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<ISummaryTo> SummaryTo { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<ISummaryBeginningYear> SummaryBeginningYear { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<ISummaryPurchase> SummaryPurchase { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal MarketValueReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal PercentWeight { get; set; }

        public DerivateMetalDetail()
        {
            Description1 = string.Empty;
            Description2 = string.Empty;
            Description3 = string.Empty;    
            Amount = 0;
            Strike = 0;
            SummaryTo = new List<ISummaryTo>();
            SummaryBeginningYear = new List<ISummaryBeginningYear>();
            SummaryPurchase = new List<ISummaryPurchase>();
            MarketValueReportingCurrency = 0;
            PercentWeight = 0;
        }

        public DerivateMetalDetail(IDerivateMetalDetail source)
        {
            Description1 = source.Description1;
            Description2 = source.Description2;
            Description3 = source.Description3;
            Amount = source.Amount;
            Strike = source.Strike;
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
            MarketValueReportingCurrency = source.MarketValueReportingCurrency;
            PercentWeight = source.PercentWeight;
        }
    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ForwardMetalOperation : IForwardMetalOperation
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Currency1 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal CurrencyValue { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal Exchange { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal ExchangeValue { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Currency2 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ExpirationDate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal CurrencyRate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal ProfitLoss { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal PercentWeight { get; set; }

        public ForwardMetalOperation()
        {
            Currency1 = string.Empty;
            CurrencyValue = 0;
            Exchange = 0;
            ExchangeValue = 0;
            Currency2 = string.Empty;
            ExpirationDate = string.Empty;  
            CurrencyRate = 0;
            ProfitLoss = 0;
            PercentWeight = 0;
        }

        public ForwardMetalOperation(IForwardMetalOperation source)
        {
            Currency1 = source.Currency1;
            CurrencyValue = source.CurrencyValue;
            Exchange = source.Exchange;
            ExchangeValue = source.ExchangeValue;
            Currency2 = source.Currency2;
            ExpirationDate = source.ExpirationDate;
            CurrencyRate = source.CurrencyRate;
            ProfitLoss = source.ProfitLoss;
            PercentWeight = source.PercentWeight;
        }

    }

    public class MetalAccountSubSection : IMetalAccountSubSection
    {

        public string Name { get; set; }

        public IList<IMetalAccount> Content { get; set; }

        public MetalAccountSubSection(string name)
        {
            Name = name;
            Content = new List<IMetalAccount>();
        }

        public MetalAccountSubSection(IMetalAccountSubSection source)
        {
            Name = source.Name;
            Content = new List<IMetalAccount>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new MetalAccount(item));
            }
        }

    }

    public class MetalSubSection : IMetalSubSection
    {

        public string Name { get; set; }

        public IList<IMetalDetail> Content { get; set; }

        public MetalSubSection(string name)
        {
            Name = name;
            Content = new List<IMetalDetail>();
        }

        public MetalSubSection(IMetalSubSection source)
        {
            Name = source.Name;
            Content = new List<IMetalDetail>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new MetalDetail(item));
            }
        }

    }

    public class DerivateMetalSubSection : IDerivateMetalSubSection
    {

        public string Name { get; set; }

        public IList<IDerivateMetalDetail> Content { get; set; }

        public DerivateMetalSubSection(string name)
        {
            Name = name;
            Content = new List<IDerivateMetalDetail>();
        }

        public DerivateMetalSubSection(IDerivateMetalSubSection source)
        {
            Name = source.Name;
            Content = new List<IDerivateMetalDetail>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new DerivateMetalDetail(item));
            }
        }

    }

    public class ForwardMetalOperationSubSection : IForwardMetalOperationSubSection
    {

        public string Name { get; set; }

        public IList<IForwardMetalOperation> Content { get; set; }

        public ForwardMetalOperationSubSection(string name)
        {
            Name = name;
            Content = new List<IForwardMetalOperation>();
        }

        public ForwardMetalOperationSubSection(IForwardMetalOperationSubSection source)
        {
            Name = source.Name;
            Content = new List<IForwardMetalOperation>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new ForwardMetalOperation(item));
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section100Content : ISection100Content
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IMetalAccountSubSection SubSection10000 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IMetalSubSection SubSection10010 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IMetalSubSection SubSection10020 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IDerivateMetalSubSection SubSection10030 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IForwardMetalOperationSubSection SubSection10040 { get; set; }

        public Section100Content()
        {
            SubSection10000 = new MetalAccountSubSection("Metals accounts");
            SubSection10010 = new MetalSubSection("Metals funds");
            SubSection10020 = new MetalSubSection("Physical metals");
            SubSection10030 = new DerivateMetalSubSection("Derivative Products on Metals");
            SubSection10040 = new ForwardMetalOperationSubSection("Forward metal operations (profit/loss)");
        }

        public Section100Content(ISection100Content source)
        {
            SubSection10000 = new MetalAccountSubSection(source.SubSection10000);
            SubSection10010 = new MetalSubSection(source.SubSection10010);
            SubSection10020 = new MetalSubSection(source.SubSection10020);
            SubSection10030 = new DerivateMetalSubSection(source.SubSection10030);
            SubSection10040 = new ForwardMetalOperationSubSection(source.SubSection10040);
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section100 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection100Content Content { get; set; }

        public Section100() : base()
        {
            Content = new Section100Content();
        }

        public Section100(Section100 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section100Content(source.Content);
            else
                Content = new Section100Content();
        }

    }

}
