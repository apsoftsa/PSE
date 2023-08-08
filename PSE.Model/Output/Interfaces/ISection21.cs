namespace PSE.Model.Output.Interfaces
{

    public interface IPossibleCommitment
    {

        string? Account { get; set; }

        string? OpeningDay { get; set; }

        string? ExpirationDate { get; set; }

        decimal? CurrentBalance { get; set; }

        decimal? MarketValueReportingCurrency { get; set; }

        int? AccruedInterestReportingCurrency { get; set; }

    }

    public interface IMortgageLoan
    {

        string? Account { get; set; }

        string? Currency { get; set; }

        string? OpeningDay { get; set; }

        string? ExpirationDate { get; set; }

        string? Rate { get; set; }

        decimal? CurrentBalance { get; set; }

        decimal? MarketValueReportingCurrency { get; set; }

        int? AccruedInterestReportingCurrency { get; set; }

    }

    public interface ISection21Content
    {

        IList<IPossibleCommitment> PossibleCommitments { get; set; }

        IList<IMortgageLoan> MortgageLoans { get; set; }

    }

}