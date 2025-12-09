using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class PerformanceEvolutionHistoryCHF: IPerformanceEvolutionHistoryCHF
    {

        public string Period { get; set; }

        public decimal? InitialAmount { get; set; }

        public decimal? FinalAmount { get; set; }

        public decimal? InputsOutputs { get; set; }

        public decimal? PercentPerformance { get; set; }

        public PerformanceEvolutionHistoryCHF()
        {
            this.Period = string.Empty;
            this.InitialAmount = 0;
            this.FinalAmount = 0;
            this.InputsOutputs = 0;  
            this.PercentPerformance = 0; 
        }

        public PerformanceEvolutionHistoryCHF(IPerformanceEvolutionHistoryCHF source)
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
    public class PerformanceEvolutionChart : IPerformanceEvolutionChart
    {

        public string Period { get; set; }

        public decimal? PercentPerformance { get; set; }

        public PerformanceEvolutionChart() 
        { 
            this.Period = string.Empty;
            this.PercentPerformance = 0;
        }

        public PerformanceEvolutionChart(IPerformanceEvolutionChart source)
        {
            this.Period = source.Period;
            this.PercentPerformance = source.PercentPerformance;
        }

    }

    public class SubSection2000Content : ISubSection2000Content
    {

        public string Name { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public string PerformanceCalculationCurrency { get; set; }

        public int PerformancesCount { get; set; }

        public IList<IPerformanceEvolutionHistoryCHF> Content { get; set; }

        public SubSection2000Content()
        {
            Name = "Performance Evolution History (CHF)";
            PerformanceCalculationCurrency = string.Empty;
            PerformancesCount = 0;  
            Content = new List<IPerformanceEvolutionHistoryCHF>();
        }

        public SubSection2000Content(ISubSection2000Content source)
        {
            Name = source.Name;
            PerformanceCalculationCurrency= source.PerformanceCalculationCurrency;  
            PerformancesCount = source.PerformancesCount;   
            Content = new List<IPerformanceEvolutionHistoryCHF>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new PerformanceEvolutionHistoryCHF(item));
            }
        }

    }

    public class SubSection2010Content : ISubSection2010Content
    {

        public string Name { get; set; }

        public IList<IPerformanceEvolutionChart> Content { get; set; }

        public SubSection2010Content()
        {
            Name = "Performance Evolution Chart";
            Content = new List<IPerformanceEvolutionChart>();
        }

        public SubSection2010Content(ISubSection2010Content source)
        {
            Name = source.Name;
            Content = new List<IPerformanceEvolutionChart>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new PerformanceEvolutionChart(item));
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section020Content : ISection020Content
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection2000Content? SubSection2000 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ISubSection2010Content? SubSection2010 { get; set; }

        public Section020Content()
        {
            SubSection2000 = null;
            SubSection2010 = null;
        }

        public Section020Content(ISection020Content source)
        {
            SubSection2000 = (source.SubSection2000 != null) ? new SubSection2000Content(source.SubSection2000) : null;
            SubSection2010 = (source.SubSection2010 != null) ? new SubSection2010Content(source.SubSection2010) : null;
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
