using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ShareEconomicSector : IShareEconomicSector
    {

        public string Sector { get; set; }

        public string Class { get; set; }

        public decimal? MarketValueReportingCurrency { get; set; }

        public decimal? PercentShares { get; set; }

        public ShareEconomicSector()
        {
            Sector = string.Empty;
            Class = string.Empty;
            MarketValueReportingCurrency = 0;
            PercentShares = 0;
        }

        public ShareEconomicSector(IShareEconomicSector source)
        {
            Sector = source.Sector;
            Class = source.Class;
            MarketValueReportingCurrency = source.MarketValueReportingCurrency;
            PercentShares = source.PercentShares;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ShareEconomicSectorChart : IShareEconomicSectorChart
    {

        public string Sector { get; set; }

        public string Class { get; set; }

        public decimal? PercentShares { get; set; }

        public ShareEconomicSectorChart()
        {
            Sector = string.Empty;
            Class = string.Empty;   
            PercentShares = 0;
        }

        public ShareEconomicSectorChart(IShareEconomicSectorChart source)
        {
            Sector = source.Sector;
            Class = source.Class;   
            PercentShares = source.PercentShares;
        }

    }

    public class ShareEconomicSectorSubSection : IShareEconomicSectorSubSection
    {

        public string Name { get; set; }

        public IList<IShareEconomicSector> Content { get; set; }

        public ShareEconomicSectorSubSection(string name)
        {
            Name = name;
            Content = new List<IShareEconomicSector>();
        }

        public ShareEconomicSectorSubSection(IShareEconomicSectorSubSection source)
        {
            Name = source.Name;
            Content = new List<IShareEconomicSector>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new ShareEconomicSector(item));
            }
        }

    }

    public class ShareEconomicSectorChartSubSection : IShareEconomicSectorChartSubSection
    {

        public string Name { get; set; }

        public IList<IShareEconomicSectorChart> Content { get; set; }

        public ShareEconomicSectorChartSubSection(string name)
        {
            Name = name;
            Content = new List<IShareEconomicSectorChart>();
        }

        public ShareEconomicSectorChartSubSection(IShareEconomicSectorChartSubSection source)
        {
            Name = source.Name;
            Content = new List<IShareEconomicSectorChart>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new ShareEconomicSectorChart(item));
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section160Content : ISection160Content
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IShareEconomicSectorSubSection? SubSection16000 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IShareEconomicSectorChartSubSection? SubSection16010 { get; set; }

        public Section160Content()
        {
            //SubSection16000 = new ShareEconomicSectorSubSection("Shares by economic sector");
            //SubSection16010 = new ShareEconomicSectorChartSubSection("Shares subdivision by economic sector chart");
            SubSection16000 = null;
            SubSection16010 = null; 
        }

        public Section160Content(ISection160Content source)
        {
            SubSection16000 = (source.SubSection16000 != null) ? new ShareEconomicSectorSubSection(source.SubSection16000) : null;
            SubSection16010 = (source.SubSection16010 != null) ? new ShareEconomicSectorChartSubSection(source.SubSection16010) : null;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section160 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection160Content Content { get; set; }

        public Section160() : base()
        {
            Content = new Section160Content();
        }

        public Section160(Section160 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section160Content(source.Content);
            else
                Content = new Section160Content();
        }

    }

}
