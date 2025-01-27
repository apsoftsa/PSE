namespace PSE.Model.Output.Interfaces
{

    public interface IShareEconomicSector
    {

        string Sector { get; set; }

        decimal? MarketValueReportingCurrency { get; set; }

        decimal? PercentShares { get; set; }

    }    

    public interface IShareEconomicSectorChart
    {

        string Sector { get; set; }

        decimal? PercentShares { get; set; }

    }

    public interface IShareEconomicSectorSubSection
    {

        string Name { get; set; }

        IList<IShareEconomicSector> Content { get; set; }

    }

    public interface IShareEconomicSectorChartSubSection
    {

        string Name { get; set; }

        IList<IShareEconomicSectorChart> Content { get; set; }

    }

    public interface ISection160Content
    {

        IShareEconomicSectorSubSection? SubSection16000 { get; set; }

        IShareEconomicSectorChartSubSection? SubSection16010 { get; set; }

    }

}