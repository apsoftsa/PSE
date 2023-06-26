﻿using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class HistoryEvolutionPerformanceCurrency: IHistoryEvolutionPerformanceCurrency
    {

        public string Period { get; set; }

        [JsonProperty(propertyName: "initial_amount")]
        public decimal? InitialAmount { get; set; }

        [JsonProperty(propertyName: "final_amount")]
        public decimal? FinalAmount { get; set; }

        [JsonProperty(propertyName: "inputs/outputs")]
        public decimal? InputsOutputs { get; set; }

        public decimal? PercentPerformance { get; set; }

        public HistoryEvolutionPerformanceCurrency()
        {
            this.Period = string.Empty;
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

        public string Period { get; set; }

        public decimal? PercentPerformance { get; set; }

        public ChartPerformanceEvolution() 
        { 
            this.Period = string.Empty;
            this.PercentPerformance = null;
        }

        public ChartPerformanceEvolution(IChartPerformanceEvolution source)
        {
            this.Period = source.Period;
            this.PercentPerformance = source.PercentPerformance;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section4Content : ISection4Content
    {

        [JsonProperty(propertyName: "historyEvolutionPerformanceCurrency", Order = 1)]
        public IList<IHistoryEvolutionPerformanceCurrency> HistoryEvolutionPerformancesCurrency { get; set; }

        [JsonProperty(propertyName: "chartPerformanceEvolution", Order = 2)]
        public IList<IChartPerformanceEvolution> ChartPerformanceEvolutions { get; set; }

        public Section4Content()
        {
            HistoryEvolutionPerformancesCurrency = new List<IHistoryEvolutionPerformanceCurrency>();
            ChartPerformanceEvolutions = new List<IChartPerformanceEvolution>();
        }

        public Section4Content(ISection4Content source)
        {
            HistoryEvolutionPerformancesCurrency = new List<IHistoryEvolutionPerformanceCurrency>();
            ChartPerformanceEvolutions = new List<IChartPerformanceEvolution>();
            if (source != null)
            {
                if (source.HistoryEvolutionPerformancesCurrency != null && source.HistoryEvolutionPerformancesCurrency.Any())
                {
                    foreach (IHistoryEvolutionPerformanceCurrency _histEvoPerfCurr in source.HistoryEvolutionPerformancesCurrency)
                    {
                        HistoryEvolutionPerformancesCurrency.Add(new HistoryEvolutionPerformanceCurrency(_histEvoPerfCurr));
                    }
                }
                if (source.ChartPerformanceEvolutions != null && source.ChartPerformanceEvolutions.Any())
                {
                    foreach (IChartPerformanceEvolution _chartPerfEvo in source.ChartPerformanceEvolutions)
                    {
                        ChartPerformanceEvolutions.Add(new ChartPerformanceEvolution(_chartPerfEvo));
                    }
                }
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section4 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection4Content Content { get; set; }

        public Section4() : base(OUTPUT_SECTION4_CODE)
        {
            Content = new Section4Content();
        }

        public Section4(Section4 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section4Content(source.Content);
            else
                Content = new Section4Content();
        }

    }

}
