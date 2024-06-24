using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Country : ICountry
    {

        [JsonProperty(propertyName: "country", NullValueHandling = NullValueHandling.Ignore)]
        public string? CountryName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? MarketValueReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? PercentShares { get; set; }

        public Country()
        {
            CountryName = null;
            MarketValueReportingCurrency = null;    
            PercentShares = null;   
        }

        public Country(ICountry source)
        {
            CountryName = source.CountryName;
            MarketValueReportingCurrency = source.MarketValueReportingCurrency;
            PercentShares = source.PercentShares;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Continent : IContinent
    {

        [JsonProperty(propertyName: "items")] // ????
        public List<ICountry> Countries { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? MarketValueReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? PercentShares { get; set; }

        public Continent()
        {
            Countries = new List<ICountry>();
            MarketValueReportingCurrency = null;
            PercentShares = null;
        }

        public Continent(IContinent source)
        {
            Countries = new List<ICountry>();
            if (source != null)
            {
                if (source.Countries != null && source.Countries.Any())
                {
                    foreach (ICountry country in source.Countries)
                    {
                        Countries.Add(new Country(country));
                    }
                }
                MarketValueReportingCurrency = source.MarketValueReportingCurrency;
                PercentShares = source.PercentShares;
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class SharesByNation : ISharesByNation
    {

        [JsonProperty(propertyName: "nations")] 
        public List<IContinent> Continents { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? TotalMarketValue { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? TotalPercentShares { get; set; }

        public SharesByNation()
        {
            Continents = new List<IContinent>();
            TotalMarketValue = null;
            TotalPercentShares = null;
        }

        public SharesByNation(ISharesByNation source)
        {
            Continents = new List<IContinent>();
            if (source != null)
            {
                if (source.Continents != null && source.Continents.Any())
                {
                    foreach (IContinent continent in source.Continents)
                    {
                        Continents.Add(new Continent(continent));
                    }
                }
                TotalMarketValue = source.TotalMarketValue;
                TotalPercentShares = source.TotalPercentShares;
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ChartSharesByContinent : IChartSharesByContinent
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Continent { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? PercentShares { get; set; }

        public ChartSharesByContinent()
        {
            Continent = null;
            PercentShares = null;
        }

        public ChartSharesByContinent(IChartSharesByContinent source)
        {
            Continent = source.Continent;
            PercentShares = source.PercentShares;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ChartSharesByCountry : IChartSharesByCountry
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Country { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? PercentShares { get; set; }

        public ChartSharesByCountry()
        {
            Country = null;
            PercentShares = null;
        }

        public ChartSharesByCountry(IChartSharesByCountry source)
        {
            Country = source.Country;
            PercentShares = source.PercentShares;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section22Content : ISection22Content
    {

        public IList<ISharesByNation> SharesByNations { get; set; }

        public IList<IChartSharesByContinent> ChartSharesByContinents { get; set; }

        public IList<IChartSharesByCountry> ChartSharesbyCountriesEuropa { get; set; }

        public IList<IChartSharesByCountry> ChartSharesbyCountriesAmerica { get; set; }

        public IList<IChartSharesByCountry> ChartSharesbyCountriesAsia { get; set; }

        public Section22Content()
        {
            SharesByNations = new List<ISharesByNation>();
            ChartSharesByContinents = new List<IChartSharesByContinent>();
            ChartSharesbyCountriesEuropa = new List<IChartSharesByCountry>();
            ChartSharesbyCountriesAmerica = new List<IChartSharesByCountry>();
            ChartSharesbyCountriesAsia = new List<IChartSharesByCountry>();
        }

        public Section22Content(ISection22Content source)
        {
            SharesByNations = new List<ISharesByNation>();
            ChartSharesByContinents = new List<IChartSharesByContinent>();
            ChartSharesbyCountriesEuropa = new List<IChartSharesByCountry>();
            ChartSharesbyCountriesAmerica = new List<IChartSharesByCountry>();
            ChartSharesbyCountriesAsia = new List<IChartSharesByCountry>();
            if (source != null)
            {
                if (source.SharesByNations != null && source.SharesByNations.Any())
                {
                    foreach (ISharesByNation shByNt in source.SharesByNations)
                    {
                        SharesByNations.Add(new SharesByNation(shByNt));
                    }
                }
                if (source.ChartSharesByContinents != null && source.ChartSharesByContinents.Any())
                {
                    foreach (IChartSharesByContinent chartByCont in source.ChartSharesByContinents)
                    {
                        ChartSharesByContinents.Add(new ChartSharesByContinent(chartByCont));
                    }
                }
                if (source.ChartSharesbyCountriesEuropa != null && source.ChartSharesbyCountriesEuropa.Any())
                {
                    foreach (IChartSharesByCountry chartByCntr in source.ChartSharesbyCountriesEuropa)
                    {
                        ChartSharesbyCountriesEuropa.Add(new ChartSharesByCountry(chartByCntr));
                    }
                }
                if (source.ChartSharesbyCountriesAmerica != null && source.ChartSharesbyCountriesAmerica.Any())
                {
                    foreach (IChartSharesByCountry chartByCntr in source.ChartSharesbyCountriesAmerica)
                    {
                        ChartSharesbyCountriesAmerica.Add(new ChartSharesByCountry(chartByCntr));
                    }
                }
                if (source.ChartSharesbyCountriesAsia != null && source.ChartSharesbyCountriesAsia.Any())
                {
                    foreach (IChartSharesByCountry chartByCntr in source.ChartSharesbyCountriesAsia)
                    {
                        ChartSharesbyCountriesAsia.Add(new ChartSharesByCountry(chartByCntr));
                    }
                }
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section22 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection22Content Content { get; set; }

        public Section22() : base()
        {
            Content = new Section22Content();
        }

        public Section22(Section22 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section22Content(source.Content);
            else
                Content = new Section22Content();
        }

    }

}
