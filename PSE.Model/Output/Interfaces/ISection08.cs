namespace PSE.Model.Output.Interfaces
{

    public interface IAccount
    {

        string AccountData { get; set; }

        string Iban { get; set; }

        string Currency { get; set; }

        string CurrentBalance { get; set; }

        string MarketValueReportingCurrency { get; set; }

        string AccruedInterestReportingCurrency { get; set; }

        string ParentAssets { get; set; }

    }

    public interface ISection8Content
    {

        IList<IAccount> Accounts { get; set; }

    }

}
