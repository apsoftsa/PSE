namespace PSE.Model.Output.Interfaces
{

    public interface ILiquidityAccount
    {

        string Description { get; set; }

        string Iban { get; set; }

        decimal? CurrentBalance { get; set; }

        decimal? MarketValueReportingCurrency { get; set; }

        decimal? PercentWeight { get; set; }

    }

    public interface ISubSection7000Content
    {

        string Name { get; set; }

        IList<ILiquidityAccount> Content { get; set; }

    }       

    public interface ILiquidityShortTermFund 
    {

        string Description1 { get; set; }

        string Description2 { get; set; }

        decimal? Quantity { get; set; }

        string Currency { get; set; }

        IList<ISummaryTo> SummaryTo {  get; set; }

        IList<ISummaryBeginningYear> SummaryBeginningYear { get; set; }

        IList<ISummaryPurchase> SummaryPurchase { get; set; }

        decimal? CapitalMarketValueReportingCurrency { get; set; }

        decimal? InterestMarketValueReportingCurrency { get; set; }

        decimal? TotalMarketValueReportingCurrency { get; set; }

        decimal? PercentWeight { get; set; }

    }

    public interface ISubSection7010Content
    {

        string Name { get; set; }

        IList<ILiquidityShortTermFund> Content { get; set; }

    }

    public interface ILiquidityFiduciaryInvestmentTemporaryDeposit
    {

        string Description1 { get; set; }

        string Description2 { get; set; }

        decimal? PercentRate { get; set; }

        string Correspondent { get; set; }

        string OpeningDate { get; set; }

        string ExpirationDate { get; set; }

        decimal? CurrentBalance { get; set; }

        decimal? MarketValueReportingCurrency { get; set; }

        decimal? AccruedInterestReportingCurrency { get; set; }

        decimal? PercentWeight { get; set; }

    }

    public interface ISubSection7020Content
    {

        string Name { get; set; }

        IList<ILiquidityFiduciaryInvestmentTemporaryDeposit> Content { get; set; }

    }

    public interface ISubSection7030Content
    {

        string Name { get; set; }

        IList<ILiquidityFiduciaryInvestmentTemporaryDeposit> Content { get; set; }
    }

    public interface ILiquidityForwardExchangeOperation
    {

        string Currency1 { get; set; }

        decimal? CurrencyValue { get; set; }

        decimal? ExchangeRate { get; set; }

        decimal? ExchangeValue { get; set; }

        string Currency2 { get; set; }

        string ExpirationDate { get; set; }

        decimal? CurrentRate { get; set; }

        decimal? ProfitLoss { get; set; }

        decimal? PercentWeight { get; set; }

    }

    public interface ISubSection7040Content
    {

        string Name { get; set; }

        IList<ILiquidityForwardExchangeOperation> Content { get; set; }

    }

    public interface ILiquidityCurrencyDerivativeProduct
    {

        string Description1 { get; set; }

        string Description2 { get; set; }

        string Description3 { get; set; }

        decimal? Amount { get; set; }

        string Strike { get; set; }

        IList<ISummaryTo> SummaryTo { get; set; }

        IList<ISummaryBeginningYear> SummaryBeginningYear { get; set; }

        IList<ISummaryPurchase> SummaryPurchase { get; set; }

        decimal? MarketValueReportingCurrency { get; set; }

        decimal? PercentWeight { get; set; }

    }

    public interface ISubSection7050Content
    {

        string Name { get; set; }

        IList<ILiquidityCurrencyDerivativeProduct> Content { get; set; }

    }

    public interface ISection070Content
    {

        ISubSection7000Content? SubSection7000 { get; set; }

        ISubSection7010Content? SubSection7010 { get; set; }

        ISubSection7020Content? SubSection7020 { get; set; }

        ISubSection7030Content? SubSection7030 { get; set; }

        ISubSection7040Content? SubSection7040 { get; set; }

        ISubSection7050Content? SubSection7050 { get; set; }

    }

}
