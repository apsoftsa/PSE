namespace PSE.Model.Output.Interfaces
{

    public interface ICountry
    {

        string? CountryName { get; set; }

        decimal? MarketValueReportingCurrency { get; set; }

        decimal? PercentShares { get; set; }

    }

    public interface IContinent
    {

        List<ICountry> Countries { get; set; }

        decimal? MarketValueReportingCurrency { get; set; }

        decimal? PercentShares { get; set; }

        string ContinentName { get; set; }

    }

    public interface ISharesByNation
    {
        
        List<IContinent> Continents { get; set; }

        decimal? TotalMarketValue { get; set; }

        decimal? TotalPercentShares { get; set; }

    }

    public interface IChartSharesByContinent
    {

        string? Continent { get; set; }

        decimal? PercentShares { get; set; }

    }

    public interface IChartSharesByCountry
    {

        string? Country { get; set; }

        decimal? PercentShares { get; set; }

    }

    public interface ISection22Content
    {

        IList<ISharesByNation> SharesByNations { get; set; }

        IList<IChartSharesByContinent> ChartSharesByContinents { get; set; }

        IList<IChartSharesByCountry> ChartSharesbyCountriesEuropa { get; set; }

        IList<IChartSharesByCountry> ChartSharesbyCountriesAmerica { get; set; }

        IList<IChartSharesByCountry> ChartSharesbyCountriesAsia { get; set; }

    }

}