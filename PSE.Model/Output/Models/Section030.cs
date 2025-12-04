using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class MultilineAssetsTo : IMultilineAssetsTo
    {
        
        public string ValueDate { get; set; }

        public decimal? ValueAsset { get; set; }

        public MultilineAssetsTo()
        {
            ValueDate = string.Empty;
            ValueAsset = 0;
        }

        public MultilineAssetsTo(IMultilineAssetsTo source)
        {
            ValueDate = source.ValueDate;
            ValueAsset = source.ValueAsset;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class MultilineKeyInformation : IMultilineKeyInformation
    {

        public string Period { get; set; }

        public int PeriodCount { get; set; }

        public IList<IMultilineAssetsTo> AssetsTo { get; set; }

        public string Currency { get; set; }

        public decimal? PercentPerformance { get; set; }        

        public MultilineKeyInformation()
        {
            Period = string.Empty;
            PeriodCount = 0;
            Currency = string.Empty;
            AssetsTo = new List<IMultilineAssetsTo>();  
            PercentPerformance = 0;
        }

        public MultilineKeyInformation(IMultilineKeyInformation source)
        {
            Period = source.Period;
            PeriodCount = source.PeriodCount;   
            Currency = source.Currency;
            AssetsTo = new List<IMultilineAssetsTo>();
            if (source.AssetsTo != null && source.AssetsTo.Any())
            {
                foreach (IMultilineAssetsTo item in source.AssetsTo)
                {
                    AssetsTo.Add(new MultilineAssetsTo(item));
                }
            }
            PercentPerformance = source.PercentPerformance;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class LinePerformanceAnalysis : ILinePerformanceAnalysis
    {

        public string ModelLine { get; set; }

        public string Currency { get; set; }

        public decimal? PercentNetContribution { get; set; }

        public string Class {  get; set; }

        public int ElementIndex { get; set; }

        public LinePerformanceAnalysis()
        {
            ModelLine = string.Empty;
            Currency = string.Empty;
            PercentNetContribution = 0;
            Class = string.Empty;  
            ElementIndex = 0;   
        }

        public LinePerformanceAnalysis(ILinePerformanceAnalysis source)
        {
            ModelLine = source.ModelLine;
            Currency = source.Currency;
            PercentNetContribution = source.PercentNetContribution;
            Class = source.Class;
            ElementIndex= source.ElementIndex;  
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class LineAllocationEvolutionChartModelLine : ILineAllocationEvolutionChartModelLine
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ModelLine { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PercentNetContribution { get; set; }

        public LineAllocationEvolutionChartModelLine()
        {
            ModelLine = string.Empty;
            PercentNetContribution = 0;
        }

        public LineAllocationEvolutionChartModelLine(ILineAllocationEvolutionChartModelLine source)
        {
            ModelLine = source.ModelLine;
            PercentNetContribution = source.PercentNetContribution;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class LineAllocationEvolutionChart : ILineAllocationEvolutionChart
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Period { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<ILineAllocationEvolutionChartModelLine> ModelLines { get; set; }

        public LineAllocationEvolutionChart()
        {
            Period = string.Empty;
            ModelLines = new List<ILineAllocationEvolutionChartModelLine>();
        }

        public LineAllocationEvolutionChart(ILineAllocationEvolutionChart source)
        {
            Period = source.Period;
            ModelLines = new List<ILineAllocationEvolutionChartModelLine>();
            if (source.ModelLines != null && source.ModelLines.Any())
            {
                foreach (ILineAllocationEvolutionChartModelLine item in source.ModelLines)
                {
                    ModelLines.Add(new LineAllocationEvolutionChartModelLine(item));
                }
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class LineAllocationEvolutionChartFlat : ILineAllocationEvolutionChartFlat {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Period { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ModelLine { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PercentNetContribution { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ElementIndex { get; set; }

        public LineAllocationEvolutionChartFlat() {
            Period = string.Empty;
            ModelLine = string.Empty;   
            PercentNetContribution = 0;
            ElementIndex = 0;            
        }

        public LineAllocationEvolutionChartFlat(ILineAllocationEvolutionChartFlat source) {
            Period = source.Period;
            ModelLine = source.ModelLine;
            PercentNetContribution = source.PercentNetContribution;
            ElementIndex = source.ElementIndex;
        }

    }

    public class SubSection3000Content : ISubSection3000Content
    {

        public string Name { get; set; }

        public IList<ILinePerformanceAnalysis> Content { get; set; }

        public SubSection3000Content()
        {
            Name = "Line Performance analysis";
            Content = new List<ILinePerformanceAnalysis>();
        }

        public SubSection3000Content(ISubSection3000Content source)
        {
            Name = source.Name;
            Content = new List<ILinePerformanceAnalysis>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new LinePerformanceAnalysis(item));
            }
        }

    }

    public class SubSection3010Content : ISubSection3010Content
    {

        public string Name { get; set; }

        public IList<ILineAllocationEvolutionChart> Content { get; set; }

        public SubSection3010Content()
        {
            Name = "Line allocation evolution chart";
            Content = new List<ILineAllocationEvolutionChart>();
        }

        public SubSection3010Content(ISubSection3010Content source)
        {
            Name = source.Name;
            Content = new List<ILineAllocationEvolutionChart>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new LineAllocationEvolutionChart(item));
            }
        }

    }

    public class SubSection3020Content : ISubSection3020Content {

        public string Name { get; set; }

        public IList<ILineAllocationEvolutionChartFlat> Content { get; set; }

        public SubSection3020Content() {
            Name = "Line allocation evolution chart flat";
            Content = new List<ILineAllocationEvolutionChartFlat>();
        }

        public SubSection3020Content(ISubSection3020Content source) {
            Name = source.Name;
            Content = new List<ILineAllocationEvolutionChartFlat>();
            if (source.Content != null && source.Content.Any()) {
                foreach (var item in source.Content)
                    Content.Add(new LineAllocationEvolutionChartFlat(item));
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section030Content : ISection030Content
    {

        public IList<IMultilineKeyInformation> KeyInformation { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection3000Content? SubSection3000 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection3010Content? SubSection3010 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection3020Content? SubSection3020 { get; set; }

        public Section030Content()
        {
            KeyInformation = new List<IMultilineKeyInformation>();
            SubSection3000 = new SubSection3000Content();
            SubSection3010 = new SubSection3010Content();
            SubSection3020 = new SubSection3020Content();
        }

        public Section030Content(ISection030Content source)
        {
            KeyInformation = new List<IMultilineKeyInformation>();
            if (source.KeyInformation != null && source.KeyInformation.Any())
            {
                foreach (var item in source.KeyInformation)
                    KeyInformation.Add(new MultilineKeyInformation(item));    
            }
            SubSection3000 = (source.SubSection3000 != null) ? new SubSection3000Content(source.SubSection3000) : null;
            SubSection3010 = (source.SubSection3010 != null) ? new SubSection3010Content(source.SubSection3010) : null;
            SubSection3020 = (source.SubSection3020 != null) ? new SubSection3020Content(source.SubSection3020) : null;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section030 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection030Content Content { get; set; }

        public Section030() : base()
        {
            Content = new Section030Content();
        }

        public Section030(Section030 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section030Content(source.Content);
            else
                Content = new Section030Content();
        }

    }

}
