using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class LiquidityAccount : ILiquidityAccount
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Iban { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal CurrentBalance { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal MarketValueReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal PercentWeight { get; set; }

        public LiquidityAccount()
        {
            this.Description = string.Empty;
            this.Iban = string.Empty;
            this.CurrentBalance = 0;
            this.MarketValueReportingCurrency = 0;
            this.PercentWeight = 0;
        }

        public LiquidityAccount(ILiquidityAccount source)
        {
            this.Description = source.Description;
            this.Iban = source.Iban;
            this.CurrentBalance = source.CurrentBalance;
            this.MarketValueReportingCurrency = source.MarketValueReportingCurrency;
            this.PercentWeight = source.PercentWeight;
        }

    }

    public class SubSection7000Content : ISubSection7000Content
    {

        public string Name { get; set; }

        public IList<ILiquidityAccount> Content { get; set; }

        public SubSection7000Content()
        {
            Name = "Accounts";
            Content = new List<ILiquidityAccount>();
        }

        public SubSection7000Content(ISubSection7000Content source)
        {
            Name = source.Name;
            Content = new List<ILiquidityAccount>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new LiquidityAccount(item));
            }
        }

    }

    public class LiquidityShortTermFund : ILiquidityShortTermFund
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
        public decimal InterestMarketValueReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal TotalMarketValueReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal PercentWeight { get; set; }

        public LiquidityShortTermFund()
        {
            Description1 = string.Empty;
            Description2 = string.Empty;    
            Quantity = 0;
            Currency = string.Empty;
            SummaryTo = new List<ISummaryTo>();
            SummaryBeginningYear = new List<ISummaryBeginningYear>();
            SummaryPurchase = new List<ISummaryPurchase>();
            CapitalMarketValueReportingCurrency = 0;
            InterestMarketValueReportingCurrency = 0;    
            TotalMarketValueReportingCurrency = 0;   
            PercentWeight = 0;
        }

        public LiquidityShortTermFund(ILiquidityShortTermFund source)
        {
            Description1 = source.Description1;
            Description2 = source.Description2;
            Quantity = source.Quantity;
            Currency = source.Currency;
            SummaryTo = new List<ISummaryTo>();
            if (source.SummaryTo != null && source.SummaryTo.Any())
            {
                foreach(var item in source.SummaryTo) 
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
            PercentWeight = source.PercentWeight;
        }

    }

    public class SubSection7010Content : ISubSection7010Content
    {

        public string Name { get; set; }

        public IList<ILiquidityShortTermFund> Content { get; set; }

        public SubSection7010Content()
        {
            Name = "Short-term funds";
            Content = new List<ILiquidityShortTermFund>();
        }

        public SubSection7010Content(ISubSection7010Content source)
        {
            Name = source.Name;
            Content = new List<ILiquidityShortTermFund>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new LiquidityShortTermFund(item));
            }
        }

    }

    public class LiquidityFiduciaryInvestmentTemporaryDeposit : ILiquidityFiduciaryInvestmentTemporaryDeposit
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description1 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description2 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal PercentRate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Correspondent { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OpeningDate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ExpirationDate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal CurrentBalance { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal MarketValueReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal AccruedInterestReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal PercentWeight { get; set; }

        public LiquidityFiduciaryInvestmentTemporaryDeposit()
        {
            Description1 = string.Empty;    
            Description2 = string.Empty;    
            PercentRate = 0;
            Correspondent = string.Empty;
            OpeningDate = string.Empty;
            ExpirationDate = string.Empty;
            CurrentBalance = 0;
            MarketValueReportingCurrency = 0;
            AccruedInterestReportingCurrency = 0;
            PercentWeight = 0;
        }

        public LiquidityFiduciaryInvestmentTemporaryDeposit(ILiquidityFiduciaryInvestmentTemporaryDeposit source)
        {
            Description1 = source.Description1;
            Description2 = source.Description2;
            PercentRate = source.PercentRate;
            Correspondent = source.Correspondent;
            OpeningDate = source.OpeningDate;
            ExpirationDate = source.ExpirationDate;
            CurrentBalance = source.CurrentBalance;
            MarketValueReportingCurrency = source.MarketValueReportingCurrency;
            AccruedInterestReportingCurrency = source.AccruedInterestReportingCurrency;
            PercentWeight = source.PercentWeight;
        }

    }

    public class SubSection7020Content : ISubSection7020Content
    {

        public string Name { get; set; }

        public IList<ILiquidityFiduciaryInvestmentTemporaryDeposit> Content { get; set; }

        public SubSection7020Content()
        {
            Name = "Fiduciary investments";
            Content = new List<ILiquidityFiduciaryInvestmentTemporaryDeposit>();
        }

        public SubSection7020Content(ISubSection7020Content source)
        {
            Name = source.Name;
            Content = new List<ILiquidityFiduciaryInvestmentTemporaryDeposit>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new LiquidityFiduciaryInvestmentTemporaryDeposit(item));
            }
        }

    }

    public class SubSection7030Content : ISubSection7030Content
    {

        public string Name { get; set; }

        public IList<ILiquidityFiduciaryInvestmentTemporaryDeposit> Content { get; set; }

        public SubSection7030Content()
        {
            Name = "Temporary deposits";
            Content = new List<ILiquidityFiduciaryInvestmentTemporaryDeposit>();
        }

        public SubSection7030Content(ISubSection7030Content source)
        {
            Name = source.Name;
            Content = new List<ILiquidityFiduciaryInvestmentTemporaryDeposit>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new LiquidityFiduciaryInvestmentTemporaryDeposit(item));
            }
        }

    }

    public class LiquidityForwardExchangeOperation : ILiquidityForwardExchangeOperation
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Currency1 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal CurrencyValue { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal ExchangeRate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal ExchangeValue { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Currency2 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ExpirationDate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal CurrentRate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal ProfitLoss { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal PercentWeight { get; set; }

        public LiquidityForwardExchangeOperation() 
        { 
            Currency1 = string.Empty;   
            CurrencyValue = 0;   
            ExchangeRate = 0;
            ExchangeValue = 0;   
            Currency2 = string.Empty;   
            ExpirationDate = string.Empty;  
            CurrentRate = 0; 
            ProfitLoss = 0;  
            PercentWeight = 0;   
        }

        public LiquidityForwardExchangeOperation(ILiquidityForwardExchangeOperation source)
        {
            Currency1 = source.Currency1;
            CurrencyValue = source.CurrencyValue;
            ExchangeRate = source.ExchangeRate;
            ExchangeValue = source.ExchangeValue;
            Currency2 = source.Currency2;
            ExpirationDate = source.ExpirationDate;
            CurrentRate = source.CurrentRate;
            ProfitLoss = source.ProfitLoss;
            PercentWeight = source.PercentWeight;
        }

    }

    public class SubSection7040Content : ISubSection7040Content
    {

        public string Name { get; set; }

        public IList<ILiquidityForwardExchangeOperation> Content { get; set; }

        public SubSection7040Content()
        {
            Name = "Forward exchange operations (profit/loss)";
            Content = new List<ILiquidityForwardExchangeOperation>();
        }

        public SubSection7040Content(ISubSection7040Content source)
        {
            Name = source.Name;
            Content = new List<ILiquidityForwardExchangeOperation>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new LiquidityForwardExchangeOperation(item));
            }
        }

    }

    public class LiquidityCurrencyDerivativeProduct : ILiquidityCurrencyDerivativeProduct
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
        public string Strike { get; set; }

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

        public LiquidityCurrencyDerivativeProduct()
        {
            Description1 = string.Empty;
            Description2 = string.Empty;
            Description3 = string.Empty;    
            Amount = 0;
            Strike = string.Empty;
            SummaryTo = new List<ISummaryTo>();
            SummaryBeginningYear = new List<ISummaryBeginningYear>();
            SummaryPurchase = new List<ISummaryPurchase>();
            MarketValueReportingCurrency = 0;
            PercentWeight = 0;
        }

        public LiquidityCurrencyDerivativeProduct(ILiquidityCurrencyDerivativeProduct source)
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

    public class SubSection7050Content : ISubSection7050Content
    {

        public string Name { get; set; }

        public IList<ILiquidityCurrencyDerivativeProduct> Content { get; set; }

        public SubSection7050Content()
        {
            Name = "Currency derivative products";
            Content = new List<ILiquidityCurrencyDerivativeProduct>();
        }

        public SubSection7050Content(ISubSection7050Content source)
        {
            Name = source.Name;
            Content = new List<ILiquidityCurrencyDerivativeProduct>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new LiquidityCurrencyDerivativeProduct(item));
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section070Content : ISection070Content
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection7000Content SubSection7000 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection7010Content SubSection7010 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection7020Content SubSection7020 { get; set; }      

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection7030Content SubSection7030 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection7040Content SubSection7040 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection7050Content SubSection7050 { get; set; }

        public Section070Content()
        {
            SubSection7000 = new SubSection7000Content();
            SubSection7010 = new SubSection7010Content();
            SubSection7020 = new SubSection7020Content();
            SubSection7030 = new SubSection7030Content();
            SubSection7040 = new SubSection7040Content();
            SubSection7050 = new SubSection7050Content();
        }

        public Section070Content(ISection070Content source)
        {
            SubSection7000 = new SubSection7000Content(source.SubSection7000);
            SubSection7010 = new SubSection7010Content(source.SubSection7010);
            SubSection7020 = new SubSection7020Content(source.SubSection7020);
            SubSection7030 = new SubSection7030Content(source.SubSection7030);
            SubSection7040 = new SubSection7040Content(source.SubSection7040);
            SubSection7050 = new SubSection7050Content(source.SubSection7050);
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section070 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection070Content Content { get; set; }

        public Section070() : base()
        {
            Content = new Section070Content();
        }

        public Section070(Section070 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section070Content(source.Content);
            else
                Content = new Section070Content();
        }

    }

}
