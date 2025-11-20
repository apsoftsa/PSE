namespace PSE.Model.Output.Interfaces;

public interface IBondDetail
{
    string Description1 { get; set; }
    string Description2 { get; set; }
    string Description3 { get; set; }
    decimal? NominalAmount { get; set; }
    string Currency { get; set; }
    string SpRating { get; set; }
    decimal? PercentRate { get; set; }
    string Coupon { get; set; }
    decimal? Duration { get; set; }
    IList<ISummaryTo> SummaryTo { get; set; }
    IList<ISummaryBeginningYear> SummaryBeginningYear { get; set; }
    IList<ISummaryPurchase> SummaryPurchase { get; set; }
    decimal? CapitalMarketValueReportingCurrency { get; set; }
    decimal? InterestMarketValueReportingCurrency { get; set; }
    decimal? TotalMarketValueReportingCurrency { get; set; }
    decimal? PercentYTD { get; set; }
    decimal? PercentWeight { get; set; }
    DateTime? IssueDate { get; set; }
}

public interface IBondFundDetail
{
    string Description1 { get; set; }
    string Description2 { get; set; }
    string Description3 { get; set; }
    decimal? Quantity { get; set; }
    string Currency { get; set; }
    IList<ISummaryTo> SummaryTo { get; set; }
    IList<ISummaryBeginningYear> SummaryBeginningYear { get; set; }
    IList<ISummaryPurchase> SummaryPurchase { get; set; }
    decimal? CapitalMarketValueReportingCurrency { get; set; }
    decimal? TotalMarketValueReportingCurrency { get; set; }
    decimal? PercentWeight { get; set; }
}

public interface IBondSubSection
{
    string Name { get; set; }
    IList<IBondDetail> Content { get; set; }
}

public interface IFundSubSection
{
    string Name { get; set; }
    IList<IBondFundDetail> Content { get; set; }
}

public interface ISection080Content
{
    IBondSubSection? SubSection8000 { get; set; }
    IBondSubSection? SubSection8010 { get; set; }
    IBondSubSection? SubSection8020 { get; set; }
    IBondSubSection? SubSection8030 { get; set; }
    IFundSubSection? SubSection8040 { get; set; }
}
