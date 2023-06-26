namespace PSE.Model.Output.Interfaces
{

    public interface IHistoryEvolutionPerformanceCurrency
    {

        string Period { get; set; }

        string InitialAmount { get; set; }

        string FinalAmount { get; set; }

        string InputsOutputs { get; set; }

        string PercentPerformance { get; set; }

    }

    public interface IChartPerformanceEvolution
    {

        string Period { get; set; }

        string PercentPerformance { get; set; }

    }

    public interface ISection4Content
    {

        IList<IHistoryEvolutionPerformanceCurrency> HistoryEvolutionPerformancesCurrency { get; set; }

        IList<IChartPerformanceEvolution> ChartPerformanceEvolutions { get; set; }

    }

}
