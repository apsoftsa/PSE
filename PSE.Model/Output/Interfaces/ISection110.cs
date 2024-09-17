namespace PSE.Model.Output.Interfaces
{   

    public interface IInvestmentDetail
    {
        string? Description1 { get; set; }
        string? Description2 { get; set; }
        string? Description3 { get; set; }
        decimal? Amount { get; set; }
        string? Currency { get; set; }
        IList<IDetailSummary>? SummaryTo { get; set; }
        IList<IDetailSummary>? SummaryBeginningYear { get; set; }
        IList<IDetailSummary>? SummaryPurchase { get; set; }
        decimal? CapitalMarketValueReportingCurrency { get; set; }
        decimal? TotalMarketValueReportingCurrency { get; set; }
        decimal? PercentWeight { get; set; }
    }

    public interface IBondInvestmentDetail : IInvestmentDetail
    {
        decimal? NominalAmount { get; set; }
        decimal? PercentRate { get; set; }
        string? Coupon { get; set; }
        decimal? InterestMarketValueReportingCurrency { get; set; }
    }

    public interface ISubSection11000
    {
        string Name { get; set; }
        IList<IInvestmentDetail>? Content { get; set; }
    }

    public interface ISubSection11010
    {
        string Name { get; set; }
        IList<IBondInvestmentDetail>? Content { get; set; }
    }

    public interface ISubSection11020
    {
        string Name { get; set; }
        IList<IInvestmentDetail>? Content { get; set; }
    }

    public interface ISubSection11030
    {
        string Name { get; set; }
        IList<IInvestmentDetail>? Content { get; set; }
    }

    public interface ISection110Content
    {
        ISubSection11000 SubSection11000 { get; set; }
        ISubSection11010 SubSection11010 { get; set; }
        ISubSection11020 SubSection11020 { get; set; }
        ISubSection11030 SubSection11030 { get; set; }
    }
}
