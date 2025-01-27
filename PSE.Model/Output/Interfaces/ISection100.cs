namespace PSE.Model.Output.Interfaces;

public interface IMetalAccount
{
    string Account { get; set; }
    decimal? CurrentBalance { get; set; }
    decimal? MarketValueReportingCurrency { get; set; }
    decimal? PercentWeight { get; set; }
}

public interface IMetalDetail
{
    string Description1 { get; set; }
    string Description2 { get; set; }
    decimal? Quantity { get; set; }
    string Currency { get; set; }
    IList<ISummaryTo> SummaryTo { get; set; }
    IList<ISummaryBeginningYear> SummaryBeginningYear { get; set; }
    IList<ISummaryPurchase> SummaryPurchase { get; set; }
    decimal? CapitalMarketValueReportingCurrency { get; set; }
    decimal? TotalMarketValueReportingCurrency { get; set; }
    decimal? PercentWeight { get; set; }
}

public interface IDerivateMetalDetail
{
    string Description1 { get; set; }
    string Description2 { get; set; }
    string Description3 { get; set; }
    decimal? Amount { get; set; }
    decimal? Strike { get; set; }
    IList<ISummaryTo> SummaryTo { get; set; }
    IList<ISummaryBeginningYear> SummaryBeginningYear { get; set; }
    IList<ISummaryPurchase> SummaryPurchase { get; set; }    
    decimal? MarketValueReportingCurrency { get; set; }
    decimal? PercentWeight { get; set; }
}

public interface IForwardMetalOperation
{
    string Currency1 { get; set; }
    decimal? CurrencyValue { get; set; }
    decimal? Exchange { get; set; }
    decimal? ExchangeValue { get; set; }
    string Currency2 { get; set; }
    string ExpirationDate { get; set; }
    decimal? CurrencyRate { get; set; }
    decimal? ProfitLoss { get; set; }
    decimal? PercentWeight { get; set; }

}

public interface IMetalAccountSubSection
{
    string Name { get; set; }
    IList<IMetalAccount> Content { get; set; }
}

public interface IMetalSubSection
{
    string Name { get; set; }
    IList<IMetalDetail> Content { get; set; }
}

public interface IDerivateMetalSubSection
{
    string Name { get; set; }
    IList<IDerivateMetalDetail> Content { get; set; }
}

public interface IForwardMetalOperationSubSection
{
    string Name { get; set; }
    IList<IForwardMetalOperation> Content { get; set; }
}

public interface ISection100Content
{

    IMetalAccountSubSection? SubSection10000 { get; set; }
    IMetalSubSection? SubSection10010 { get; set; }
    IMetalSubSection? SubSection10020 { get; set; }
    IDerivateMetalSubSection? SubSection10030 { get; set; }
    IForwardMetalOperationSubSection? SubSection10040 { get; set; }

}
