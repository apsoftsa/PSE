namespace PSE.Model.Output.Interfaces
{

    public interface IProfitLossOperation
    {

        string AmountLoss { get; set; }

        string CurrencyLoss { get; set; }

        string Change { get; set; }

        string Amount2 { get; set; }

        string Currency2 { get; set; }

        string ExpirationDate { get; set; }

        string CurrentExchangeRate { get; set; }

        string ProfitlossReportingCurrency { get; set; }

        string PercentAssets { get; set; }

    }

    public interface ISection11Content
    {

        IList<IProfitLossOperation> Operations { get; set; }

    }

}
