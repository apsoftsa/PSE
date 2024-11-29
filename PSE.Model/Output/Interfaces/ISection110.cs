namespace PSE.Model.Output.Interfaces
{

    public interface IInvestmentBase
    {
        string Description1 { get; set; }
        string Description2 { get; set; }
        string Description3 { get; set; }
        string Currency { get; set; }
        IList<ISummaryTo>? SummaryTo { get; set; }
        IList<ISummaryBeginningYear>? SummaryBeginningYear { get; set; }
        IList<ISummaryPurchase>? SummaryPurchase { get; set; }
        decimal CapitalMarketValueReportingCurrency { get; set; }
        decimal TotalMarketValueReportingCurrency { get; set; }
        decimal PercentWeight { get; set; }
    }

    public interface IInvestmentDetail : IInvestmentBase
    {
        decimal Amount { get; set; }
    }

    public interface IBondInvestmentDetail : IInvestmentBase
    {
        decimal NominalAmount { get; set; }
        decimal PercentRate { get; set; }
        string Coupon { get; set; }
        decimal InterestMarketValueReportingCurrency { get; set; }
    }

    public interface ISubSection11000
    {
        string Name { get; set; }
        IList<IInvestmentDetail> Content { get; set; }
    }

    public interface ISubSection11010
    {
        string Name { get; set; }
        IList<IBondInvestmentDetail> Content { get; set; }
    }

    public interface ISubSection11020
    {
        string Name { get; set; }
        IList<IInvestmentDetail> Content { get; set; }
    }

    public interface ISubSection11030
    {
        string Name { get; set; }
        IList<IInvestmentDetail> Content { get; set; }
    }

    public interface ISection110Content
    {
        ISubSection11000 SubSection11000 { get; set; }
        ISubSection11010 SubSection11010 { get; set; }
        ISubSection11020 SubSection11020 { get; set; }
        ISubSection11030 SubSection11030 { get; set; }
    }

}
