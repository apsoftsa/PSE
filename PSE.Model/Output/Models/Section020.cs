using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class HistoryEvolutionPerformanceCurrency: IHistoryEvolutionPerformanceCurrency
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Period { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? InitialAmount { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? FinalAmount { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? InputsOutputs { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PercentPerformance { get; set; }

        public HistoryEvolutionPerformanceCurrency()
        {
            this.Period = null;
            this.InitialAmount = null;
            this.FinalAmount = null;
            this.InputsOutputs = null;  
            this.PercentPerformance = null; 
        }

        public HistoryEvolutionPerformanceCurrency(IHistoryEvolutionPerformanceCurrency source)
        {
            this.Period = source.Period;
            this.InitialAmount = source.InitialAmount;
            this.FinalAmount = source.FinalAmount;
            this.InputsOutputs = source.InputsOutputs;
            this.PercentPerformance = source.PercentPerformance;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ChartPerformanceEvolution : IChartPerformanceEvolution
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Period { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PercentPerformance { get; set; }

        public ChartPerformanceEvolution() 
        { 
            this.Period = null;
            this.PercentPerformance = null;
        }

        public ChartPerformanceEvolution(IChartPerformanceEvolution source)
        {
            this.Period = source.Period;
            this.PercentPerformance = source.PercentPerformance;
        }

    }

    public class SubSection2000Content : ISubSection2000Content
    {

        public string Name { get; set; }

        public IList<IHistoryEvolutionPerformanceCurrency> Content { get; set; }

        public SubSection2000Content()
        {
            Name = "Performance Evolution History (CHF)";
            Content = new List<IHistoryEvolutionPerformanceCurrency>();
        }

        public SubSection2000Content(ISubSection2000Content source)
        {
            Name = source.Name;
            Content = new List<IHistoryEvolutionPerformanceCurrency>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new HistoryEvolutionPerformanceCurrency(item));
            }
        }

    }

    public class SubSection2010Content : ISubSection2010Content
    {

        public string Name { get; set; }

        public IList<IChartPerformanceEvolution> Content { get; set; }

        public SubSection2010Content()
        {
            Name = "Performance Evolution Chart";
            Content = new List<IChartPerformanceEvolution>();
        }

        public SubSection2010Content(ISubSection2010Content source)
        {
            Name = source.Name;
            Content = new List<IChartPerformanceEvolution>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new ChartPerformanceEvolution(item));
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section020Content : ISection020Content
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection2000Content SubSection2000 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection2010Content SubSection2010 { get; set; }

        public Section020Content()
        {
            SubSection2000 = new SubSection2000Content();
            SubSection2010 = new SubSection2010Content();
        }

        public Section020Content(ISection020Content source)
        {
            SubSection2000 = new SubSection2000Content(source.SubSection2000);
            SubSection2010 = new SubSection2010Content(source.SubSection2010);
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section020 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection020Content Content { get; set; }

        public Section020() : base()
        {
            Content = new Section020Content();
        }

        public Section020(Section020 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section020Content(source.Content);
            else
                Content = new Section020Content();
        }

    }

}
