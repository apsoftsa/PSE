using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ShareByCountry : IShareByCountry
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Country { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? MarketValueReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PercentShares { get; set; }

        public ShareByCountry()
        {
            Country = string.Empty;
            MarketValueReportingCurrency = null;
            PercentShares = null;
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

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Nation { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<IShareByCountry>? Content { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? MarketValueReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PercentShares { get; set; }

        public ShareByNation()
        {
            Nation = string.Empty;
            Content = new List<IShareByCountry>();
            MarketValueReportingCurrency = null;
            PercentShares = null;
        }

        public ShareByNation(IShareByNation source)
        {
            Nation = source.Nation;
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

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class TotalShares : ITotalShares
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? TotalSharesValue { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? TotalMarketValueReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? TotalPercentShares { get; set; }

        public TotalShares()
        {
            TotalSharesValue = null;
            TotalMarketValueReportingCurrency = null;
            TotalPercentShares = null;
        }

        public TotalShares(ITotalShares source)
        {
            TotalSharesValue = source.TotalSharesValue;
            TotalMarketValueReportingCurrency = source.TotalMarketValueReportingCurrency;
            TotalPercentShares = source.TotalPercentShares;
        }

    }

    public class SubSection17000 : ISubSection17000
    {
        public string? Name { get; set; }
        public IList<IShareByNation>? Content { get; set; }
        public ITotalShares? TotalSharesData { get; set; }

        public SubSection17000(string name)
        {
            Name = name;
            Content = new List<IShareByNation>();
            TotalSharesData = new TotalShares();
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
            TotalSharesData = new TotalShares(source.TotalSharesData);
        }
    }

    public class ShareByNationChart : IShareByNationChart
    {
        public string? Nation { get; set; }
        public decimal? PercentShares { get; set; }

        public ShareByNationChart()
        {
            Nation = string.Empty;
            PercentShares = null;
        }

        public ShareByNationChart(IShareByNationChart source)
        {
            Nation = source.Nation;
            PercentShares = source.PercentShares;
        }

    }

    public class SubSection17010 : ISubSection17010
    {
        public string? Name { get; set; }
        public IList<IShareByNationChart>? Content { get; set; }

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
            SubSection17000 = new SubSection17000("Shares by nation");
            SubSection17010 = new SubSection17010("Shares by nations chart");
        }

        public Section170Content(ISection170Content source)
        {
            SubSection17000 = new SubSection17000(source.SubSection17000);
            SubSection17010 = new SubSection17010(source.SubSection17010);
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
