namespace PSE.Model.Output.Interfaces
{

    public interface IAccountAndDepositReport
    {

        string? Object { get; set; }

        string? Description { get; set; }

        string? AddressBook { get; set; }

        string? Currency { get; set; }

        decimal? CurrentBalance { get; set; }

        decimal? MarketValueReportingCurrency { get; set; }

        string? Total { get; set; }

        string? TotalAsset { get; set; }

        string? TotalAddressBook { get; set; }

        string? TotalAssetsNotTransferred { get; set; }

        decimal? TotalMarketValueReportingCurrency { get; set; }

    }

    public interface ISubSection19000
    {
        string? Name { get; set; }
        IList<IAccountAndDepositReport>? Content { get; set; }
    }

    public interface ISubSection19010
    {
        string? Name { get; set; }
        IList<IAccountAndDepositReport>? Content { get; set; }
    }

    public interface ISection190Content
    {
        ISubSection19000? SubSection19000 { get; set; }
        ISubSection19010? SubSection19010 { get; set; }
    }

}