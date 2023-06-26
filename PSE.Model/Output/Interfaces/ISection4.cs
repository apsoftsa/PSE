namespace PSE.Model.Output.Interfaces
{

    public interface IHistoryEvolutionPerformanceCurrency
    {

        string Period { get; set; }

        decimal? InitialAmount { get; set; }

        decimal? FinalAmount { get; set; }

        decimal? InputsOutputs { get; set; }

        decimal? PercentPerformance { get; set; }

    }

    public interface IChartPerformanceEvolution
    {

        string Period { get; set; }

        decimal? PercentPerformance { get; set; }

    }

    public interface ISection4Content
    {

        IList<IHistoryEvolutionPerformanceCurrency> HistoryEvolutionPerformancesCurrency { get; set; }

        IList<IChartPerformanceEvolution> ChartPerformanceEvolutions { get; set; }

    }

}
