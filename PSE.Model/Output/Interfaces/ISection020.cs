namespace PSE.Model.Output.Interfaces
{

    public interface IPerformanceEvolutionHistoryCHF
    {

        string Period { get; set; }

        decimal? InitialAmount { get; set; }

        decimal? FinalAmount { get; set; }

        decimal? InputsOutputs { get; set; }

        decimal? PercentPerformance { get; set; }

    }

    public interface IPerformanceEvolutionChart
    {

        string Period { get; set; }

        decimal? PercentPerformance { get; set; }

    }

    public interface ISubSection2000Content
    {

        string Name { get; set; }

        string PerformanceCalculationCurrency { get; set; } 

        IList<IPerformanceEvolutionHistoryCHF> Content { get; set; }

    }

    public interface ISubSection2010Content
    {

        string Name { get; set; }

        IList<IPerformanceEvolutionChart> Content { get; set; }

    }

    public interface ISection020Content
    {

        ISubSection2000Content? SubSection2000 { get; set; }

        ISubSection2010Content? SubSection2010 { get; set; }

    }

}
