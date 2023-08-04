namespace PSE.Model.Output.Interfaces
{

    public interface IFiduciaryInvestmentAccount
    {

        string? Account { get; set; }

        string? Currency { get; set; }

        string? NoDeposit { get; set; }

        decimal? PercentInterest { get; set; }

        string? Correspondent { get; set; }

        string? OpeningDate { get; set; }

        string? ExpirationDate { get; set; }

        decimal? FaceValue { get; set; }

        decimal? MarketValueReportingCurrency { get; set; }

        decimal? AccruedInterestReportingCurrency { get; set; }

        decimal? PercentAsset { get; set; }

    }

    public interface ISection10Content
    {

        IList<IFiduciaryInvestmentAccount> Accounts { get; set; }

    }

}
