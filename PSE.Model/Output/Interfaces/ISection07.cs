namespace PSE.Model.Output.Interfaces
{

    public interface IInvestment
    {

        bool IsTotal { get; set; }

        string TotalCurrency { get; set; }

        string Currency { get; set; }

        string Amount { get; set; }

        string Exchange { get; set; }

        string MarketValueReportingCurrency { get; set; }

        string PercentAsset { get; set; }

    }

    public interface IChartInvestment
    {

        string PercentType { get; set; }    

        string Currency { get; set; }

        string PercentValue { get; set; }

    }

    public interface ISection7Content
    {

        IList<IInvestment> Investments { get; set; }

        IList<IChartInvestment> ChartInvestments { get; set; }

    }

}
