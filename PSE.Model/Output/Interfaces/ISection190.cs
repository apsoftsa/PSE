namespace PSE.Model.Output.Interfaces
{

    public interface IObjectReportsTransferredToAdministration
    {

        string Object { get; set; }

        string Description { get; set; }

        string AddressBook { get; set; }

        string? Currency { get; set; }

        decimal? CurrentBalance { get; set; }

        decimal MarketValueReportingCurrency { get; set; }

    }

    public interface IReportsTransferredToAdministration
    {

        string TotalAsset { get; set; }

        string TotalAddressBook { get; set; }

        decimal? TotalMarketValueReportingCurrency { get; set; }

        string TotalAssetsNotTransferred { get; set; }

        decimal? TotalNotTransferredMarketValueReportingCurrency { get; set; }               

        List<IObjectReportsTransferredToAdministration> Objects { get; set; }

    }

    public interface IObjectReportsNotTransferredToAdministration
    {

        string Object { get; set; }

        string Description { get; set; }

        string AddressBook { get; set; }

        string Currency { get; set; }

        decimal? CurrentBalance { get; set; }

        decimal? MarketValueReportingCurrency { get; set; }

    }

    public interface IReportsNotTransferredToAdministration
    {

        string TotalAssetsNotTransferred { get; set; }

        decimal? TotalNotTransferredMarketValueReportingCurrency { get; set; }

        string TotalAsset { get; set; }

        decimal? TotalMarketValueReportingCurrency { get; set; }

        List<IObjectReportsNotTransferredToAdministration> Objects { get; set; }    

    }

    public interface ISubSection19000
    {
        string Name { get; set; }
        IList<IReportsNotTransferredToAdministration> Content { get; set; }
    }

    public interface ISubSection19010
    {
        string Name { get; set; }
        IList<IReportsTransferredToAdministration> Content { get; set; }
    }

    public interface ISection190Content
    {
        ISubSection19000? SubSection19000 { get; set; }
        ISubSection19010? SubSection19010 { get; set; }
    }

}