namespace PSE.Model.Output.Interfaces
{

    public interface IAccount
    {

        string? AccountData { get; set; }

        string? Iban { get; set; }

        string? Currency { get; set; }

        decimal? CurrentBalance { get; set; }

        decimal? MarketValueReportingCurrency { get; set; }

        decimal? AccruedInterestReportingCurrency { get; set; }

        decimal? PercentAsset { get; set; }

    }

    public interface ISection8Content
    {

        IList<IAccount> Accounts { get; set; }

    }

}
