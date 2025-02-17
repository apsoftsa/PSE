namespace PSE.Model.Output.Interfaces;

public interface IShareDetail
{
    string Description1 { get; set; }
    string Description2 { get; set; }
    decimal? Amount { get; set; }
    string Currency { get; set; }
    IList<ISummaryTo> SummaryTo { get; set; }
    IList<ISummaryBeginningYear> SummaryBeginningYear { get; set; }
    IList<ISummaryPurchase> SummaryPurchase { get; set; }
    decimal? CapitalMarketValueReportingCurrency { get; set; }
    decimal? PercentWeight { get; set; }
}

public interface IEquityFundDetail : IShareDetail
{
    string Description3 { get; set; }
    decimal? TotalMarketValueReportingCurrency { get; set; }
}

public interface IDerivateDetail : IEquityFundDetail { }

public interface IShareSubSection
{
    string Name { get; set; }
    IList<IShareDetail> Content { get; set; }
}

public interface IEquityFundSubSection
{
    string Name { get; set; }
    IList<IEquityFundDetail> Content { get; set; }
}

public interface IDerivateSubSection
{
    string Name { get; set; }
    IList<IDerivateDetail> Content { get; set; }
}


public interface ISection090Content
{
    IShareSubSection? SubSection9010 { get; set; }
    IEquityFundSubSection? SubSection9020 { get; set; }
    IDerivateSubSection? SubSection9030 { get; set; }
}
