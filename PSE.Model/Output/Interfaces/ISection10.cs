namespace PSE.Model.Output.Interfaces
{

    public interface IFiduciaryInvestmentAccount
    {

        string Account { get; set; }

        string Currency { get; set; }

        string NoDeposit { get; set; }

        string PercentInterest { get; set; }

        string Correspondent { get; set; }

        string OpeningDate { get; set; }

        string ExpirationDate { get; set; }

        string FaceValue { get; set; }

        string MarketValueReportingCurrency { get; set; }

        string AccruedInterestReportingCurrency { get; set; }

        string PercentAssets { get; set; }

    }

    public interface ISection10Content
    {

        IList<IFiduciaryInvestmentAccount> Accounts { get; set; }

    }

}
