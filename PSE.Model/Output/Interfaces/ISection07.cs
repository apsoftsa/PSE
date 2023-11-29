namespace PSE.Model.Output.Interfaces
{

    public interface IInvestment
    {

        string? Currency { get; set; }

        decimal? Amount { get; set; }

        decimal? Exchange { get; set; }

        decimal? MarketValueReportingCurrency { get; set; }

        decimal? PercentAsset { get; set; }

    }

    public interface IChartInvestment
    {       

        string? Currency { get; set; }

        decimal? PercentAsset { get; set; }

    }

    public interface ISection7Content
    {

        IList<IInvestment> Investments { get; set; }

        IList<IChartInvestment> ChartInvestments { get; set; }

    }

}
