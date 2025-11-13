using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ShareByCountry : IShareByCountry
    {

        public string Country { get; set; }

        public decimal? MarketValueReportingCurrency { get; set; }

        public decimal? PercentShares { get; set; }

        public ShareByCountry()
        {
            Country = string.Empty;
            MarketValueReportingCurrency = 0;
            PercentShares = 0;
        }

        public ShareByCountry(IShareByCountry source)
        {
            Country = source.Country;
            MarketValueReportingCurrency = source.MarketValueReportingCurrency;
            PercentShares = source.PercentShares;
        }
    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ShareByNation : IShareByNation
    {

        public string Nation { get; set; }

        public string Class { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<IShareByCountry>? Content { get; set; }

        public decimal? MarketValueReportingCurrency { get; set; }

        public decimal? PercentShares { get; set; }

        public ShareByNation()
        {
            Nation = string.Empty;
            Class = string.Empty;   
            Content = new List<IShareByCountry>();
            MarketValueReportingCurrency = 0;
            PercentShares = 0;
        }

        public ShareByNation(IShareByNation source)
        {
            Nation = source.Nation;
            Class = source.Class;
            Content = new List<IShareByCountry>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new ShareByCountry(item));
            }
            MarketValueReportingCurrency = source.MarketValueReportingCurrency;
            PercentShares = source.PercentShares;
        }

    }

    public class SubSection17000 : ISubSection17000
    {
        public string Name { get; set; }
        public IList<IShareByNation> Content { get; set; }

        public SubSection17000(string name)
        {
            Name = name;
            Content = new List<IShareByNation>();
        }

        public SubSection17000(ISubSection17000 source)
        {
            Name = source.Name;
            Content = new List<IShareByNation>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new ShareByNation(item));
            }
        }
    }

    public class ShareByNationChart : IShareByNationChart
    {
        public string Nation { get; set; }
        public decimal? PercentShares { get; set; }

        public ShareByNationChart()
        {
            Nation = string.Empty;
            PercentShares = 0;
        }

        public ShareByNationChart(IShareByNationChart source)
        {
            Nation = source.Nation;
            PercentShares = source.PercentShares;
        }

    }

    public class SubSection17010 : ISubSection17010
    {
        public string Name { get; set; }
        public IList<IShareByNationChart> Content { get; set; }

        public SubSection17010(string name)
        {
            Name = name;
            Content = new List<IShareByNationChart>();
        }

        public SubSection17010(ISubSection17010 source)
        {
            Name = source.Name;
            Content = new List<IShareByNationChart>();
            if (source.Content != null && source.Content.Any())
            {
                foreach (var item in source.Content)
                    Content.Add(new ShareByNationChart(item));
            }
        }
    }


    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section170Content : ISection170Content
    {
        public ISubSection17000? SubSection17000 { get; set; }
        public ISubSection17010? SubSection17010 { get; set; }

        public Section170Content()
        {
            //SubSection17000 = new SubSection17000("Shares by nation");
            //SubSection17010 = new SubSection17010("Shares by nations chart");
            SubSection17000 = null;
            SubSection17010 = null; 
        }

        public Section170Content(ISection170Content source)
        {
            SubSection17000 = (source.SubSection17000 != null) ? new SubSection17000(source.SubSection17000) : null;
            SubSection17010 = (source.SubSection17010 != null) ? new SubSection17010(source.SubSection17010) : null;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section170 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection170Content Content { get; set; }

        public Section170() : base()
        {
            Content = new Section170Content();
        }

        public Section170(Section170 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section170Content(source.Content);
            else
                Content = new Section170Content();
        }

    }

}
