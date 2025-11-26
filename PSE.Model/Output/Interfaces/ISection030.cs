using System.Text.Json.Serialization;

namespace PSE.Model.Output.Interfaces
{

    public interface IMultilineAssetsTo
    {

        string ValueDate { get; set; }

        decimal? ValueAsset { get; set; }

    }

    public interface IMultilineKeyInformation
    {

        string Period {  get; set; }    

        IList<IMultilineAssetsTo> AssetsTo { get; set; }   

        string Currency {  get; set; }  

        decimal? PercentPerformance { get; set; }    

    }

    public interface ILinePerformanceAnalysis
    {

        string ModelLine { get; set; }

        string Currency { get; set; }

        decimal? PercentNetContribution { get; set; }

        string Class {  get; set; }

        int ElementIndex { get; set; }

    }

    public interface ILineAllocationEvolutionChartModelLine
    {
        
        string ModelLine { get; set; }

        decimal? PercentNetContribution { get; set; }
        
    }

    public interface ILineAllocationEvolutionChart
    {

        string Period { get; set; }

        IList<ILineAllocationEvolutionChartModelLine> ModelLines { get; set; }

    }

    public interface ILineAllocationEvolutionChartFlat {

        string Period { get; set; }

        string ModelLine { get; set; }

        decimal? PercentNetContribution { get; set; }

        int ElementIndex { get; set; }  

    }

    public interface ISubSection3000Content
    {

        string Name { get; set; }

        IList<ILinePerformanceAnalysis> Content { get; set; }

    }

    public interface ISubSection3010Content
    {

        string Name { get; set; }

        IList<ILineAllocationEvolutionChart> Content { get; set; }

    }

    public interface ISubSection3020Content {

        string Name { get; set; }

        IList<ILineAllocationEvolutionChartFlat> Content { get; set; }

    }

    public interface ISection030Content
    {

        IList<IMultilineKeyInformation> KeyInformation { get; set; }

        ISubSection3000Content? SubSection3000 { get; set; }

        ISubSection3010Content? SubSection3010 { get; set; }

        ISubSection3020Content? SubSection3020 { get; set; }

    }

}
