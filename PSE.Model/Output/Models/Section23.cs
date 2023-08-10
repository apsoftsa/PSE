using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class EconSector : IEconSector
    {

        public string? Sector { get; set; }

        public decimal? MarketValueReportingCurrency { get; set; }

        public decimal? PercentShares { get; set; }

        public EconSector()
        {
            Sector = null;
            MarketValueReportingCurrency = null;
            PercentShares = null;
        }

        public EconSector(IEconSector source)
        {
            Sector = source.Sector;
            MarketValueReportingCurrency = source.MarketValueReportingCurrency;
            PercentShares = source.PercentShares;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ActionByEconSector : IActionByEconSector
    {

        public List<IEconSector> Sectors { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? TotalMarketValueReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? TotalPercentShares { get; set; }

        public ActionByEconSector()
        {
            Sectors = new List<IEconSector>();
            TotalMarketValueReportingCurrency = null;
            TotalPercentShares = null;
        }

        public ActionByEconSector(IActionByEconSector source)
        {
            Sectors = new List<IEconSector>();
            if (source != null)
            {
                if (source.Sectors != null && source.Sectors.Any())
                {
                    foreach (IEconSector _sector in source.Sectors)
                    {
                        Sectors.Add(new EconSector(_sector));
                    }
                }
                TotalMarketValueReportingCurrency = source.TotalMarketValueReportingCurrency;
                TotalPercentShares = source.TotalPercentShares;
            }

        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class EconominalSector : IEconominalSector
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Sector { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? PercentShares { get; set; }

        public EconominalSector()
        {
            Sector = null;
            PercentShares = null;
        }

        public EconominalSector(IEconominalSector source)
        {
            Sector = source.Sector;
            PercentShares = source.PercentShares;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section23Content : ISection23Content
    {

        public IList<IActionByEconSector> ActionByEconSector { get; set; }

        public IList<IEconominalSector> ChartGraphicEconomicalSector { get; set; }

        public Section23Content()
        {
            ActionByEconSector = new List<IActionByEconSector>();
            ChartGraphicEconomicalSector = new List<IEconominalSector>();
        }

        public Section23Content(ISection23Content source)
        {
            ActionByEconSector = new List<IActionByEconSector>();
            ChartGraphicEconomicalSector = new List<IEconominalSector>();
            if (source != null)
            {
                if (source.ActionByEconSector != null && source.ActionByEconSector.Any())
                {
                    foreach (IActionByEconSector _actByEconSec in source.ActionByEconSector)
                    {
                        ActionByEconSector.Add(new ActionByEconSector(_actByEconSec));
                    }
                }
                if (source.ChartGraphicEconomicalSector != null && source.ChartGraphicEconomicalSector.Any())
                {
                    foreach (IEconominalSector _ecSect in source.ChartGraphicEconomicalSector)
                    {
                        ChartGraphicEconomicalSector.Add(new EconominalSector(_ecSect));
                    }
                }

            }

        }

    }


    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section23 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection23Content Content { get; set; }

        public Section23() : base()
        {
            Content = new Section23Content();
        }

        public Section23(Section23 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section23Content(source.Content);
            else
                Content = new Section23Content();
        }

    }

}
