using System.Text.Json.Serialization;

namespace PSE.Model.Output.Interfaces
{

    public interface IMultilineManagement
    {

        bool IsTotal { get; set; }   

        string ManagementLine { get; set; }

        string TotalLine { get; set; }

        string Currency { get; set; }

        string MarketValueReportingCurrency { get; set; }

        string PercentWeight { get; set; }

        string PercentPerformance { get; set; }

    }

    public interface IChartMultilineManagement
    {

        string Currency { get; set; }

        string ManagementLine { get; set; }

        string PercentCurrency { get; set; }

    }

    public interface ISection5Content
    {

        IList<IMultilineManagement> MultilinesManagement { get; set; }

        IList<IChartMultilineManagement> ChartMultilinesManagement { get; set; }

    }

}
