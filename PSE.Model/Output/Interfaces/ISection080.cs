namespace PSE.Model.Output.Interfaces;

public interface IBondDetail
{
    string? Description1 { get; set; }
    string? Description2 { get; set; }
    string? Description3 { get; set; }
    decimal? NominalAmount { get; set; }
    string? Currency { get; set; }
    string? SpRating { get; set; }
    decimal? PercentRate { get; set; }
    string? Coupon { get; set; }
    decimal? Duration { get; set; }
    IList<IDetailSummary> SummaryTo { get; set; }
    IList<IDetailSummary> SummaryBeginningYear { get; set; }
    IList<IDetailSummary> SummaryPurchase { get; set; }
    decimal? CapitalMarketValueReportingCurrency { get; set; }
    decimal? InterestMarketValueReportingCurrency { get; set; }
    decimal? TotalMarketValueReportingCurrency { get; set; }
    decimal? PercentYTD { get; set; }
    decimal? PercentWeight { get; set; }
}

public interface IBondFundDetail
{
    string? Description1 { get; set; }
    string? Description2 { get; set; }
    string? Description3 { get; set; }
    decimal? Quantity { get; set; }
    string? Currency { get; set; }
    IList<IDetailSummary> SummaryTo { get; set; }
    IList<IDetailSummary> SummaryBeginningYear { get; set; }
    IList<IDetailSummary> SummaryPurchase { get; set; }
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
    IBondSubSection Subsection8000 { get; set; }
    IBondSubSection Subsection8010 { get; set; }
    IBondSubSection Subsection8020 { get; set; }
    IBondSubSection SubSection8030 { get; set; }
    IFundSubSection Subsection8040 { get; set; }
}
