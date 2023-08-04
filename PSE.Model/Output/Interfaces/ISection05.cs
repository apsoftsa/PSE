using System.Text.Json.Serialization;

namespace PSE.Model.Output.Interfaces
{

    public interface IMultilineManagement
    {

        bool IsTotal { get; set; }

        string? ManagementLine { get; set; }

        string? Currency { get; set; }

        decimal? MarketValueReportingCurrency { get; set; }

        decimal? PercentWeight { get; set; }

        int? PercentPerformance { get; set; }

    }

    public interface IChartMultilineManagement
    {

        string? Currency { get; set; }

        string? ManagementLine { get; set; }

        int? Percent { get; set; }

    }

    public interface ISection5Content
    {

        IList<IMultilineManagement> MultilinesManagement { get; set; }

        IList<IChartMultilineManagement> ChartMultilinesManagement { get; set; }

    }

}
