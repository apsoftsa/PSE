namespace PSE.Model.Output.Interfaces
{

    public interface IEconSector
    {

        string? Sector { get; set; }

        decimal? MarketValueReportingCurrency { get; set; }

        decimal? PercentShares { get; set; }

    }

    public interface IActionByEconSector
    {

        List<IEconSector> Sectors { get; set; }

        decimal? TotalMarketValueReportingCurrency { get; set; }

        int? TotalPercentShares { get; set; }

    }

    public interface IEconominalSector
    {

        string? Sector { get; set; }

        decimal? PercentShares { get; set; }

    }

    public interface ISection23Content
    {

        IList<IActionByEconSector> ActionByEconSector { get; set; }

        IList<IEconominalSector> ChartGraphicEconomicalSector { get; set; }

    }

}