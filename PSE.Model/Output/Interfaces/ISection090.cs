namespace PSE.Model.Output.Interfaces;

public interface IShareDetail
{
    string? Description1 { get; set; }
    string? Description2 { get; set; }
    string? Description3 { get; set; }
    decimal? Amount { get; set; }
    string? Currency { get; set; }
    IList<IDetailSummary> SummaryTo { get; set; }
    IList<IDetailSummary> SummaryBeginningYear { get; set; }
    IList<IDetailSummary> SummaryPurchase { get; set; }
    decimal? CapitalMarketValueReportingCurrency { get; set; }
    decimal? TotalMarketValueReportingCurrency { get; set; }
    decimal? PercentWeight { get; set; }
}

public interface IShareSubSection
{
    string Name { get; set; }
    IList<IShareDetail> Content { get; set; }
}

public interface ISection090Content
{
    IShareSubSection Subsection9010 { get; set; }
    IShareSubSection Subsection9020 { get; set; }
    IShareSubSection SubSection9030 { get; set; }
}
