namespace PSE.Model.Output.Interfaces
{

    public interface IProfitLossOperation
    {

        decimal? AmountLoss { get; set; }

        string? CurrencyLoss { get; set; }

        decimal? Change { get; set; }

        decimal? Amount2 { get; set; }

        string? Currency2 { get; set; }

        string? ExpirationDate { get; set; }

        decimal? CurrentExchangeRate { get; set; }

        decimal? ProfitlossReportingCurrency { get; set; }

        decimal? PercentAssets { get; set; }

    }

    public interface ISection11Content
    {

        IList<IProfitLossOperation> Operations { get; set; }

    }

}
