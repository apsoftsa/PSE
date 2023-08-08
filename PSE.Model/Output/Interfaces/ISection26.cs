namespace PSE.Model.Output.Interfaces
{

    public interface IOverviewPortfolio
    {

        string? CustomerName { get; set; }

        string? CustomerNumber { get; set; }

        string? Portfolio { get; set; }

        string? Service { get; set; }

        string? RiskProfile { get; set; }

    }

    public interface IGraphicalInvestment
    {

        string? TypeInvestment { get; set; }

        int? Percent { get; set; }

    }

    public interface ITypeInvestment
    {

        string? InvestmentType { get; set; }

        decimal? MarketValueReportingCurrency { get; set; }

    }

    public interface IAnyCommitments
    {

        decimal? MarketValueReportingCurrency { get; set; }

    }

    public interface ISection26Content
    {

        IList<IOverviewPortfolio> OverviewPortfolio { get; set; }

        IList<IGraphicalInvestment> ChartGraphicalInvestment { get; set; }

        IList<ITypeInvestment> Investment { get; set; }

        IList<IAnyCommitments> InformationPosition { get; set; }

    }

}