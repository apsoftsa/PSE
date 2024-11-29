namespace PSE.Model.Output.Interfaces
{

    public interface ISettled
    {

        string Date { get; set; }    

        string Time { get; set; }

    }

    public interface ISummaryTo
    {
        string ValueDate { get; set; }
        decimal ValuePrice { get; set; }
        decimal PercentPrice { get; set; }
        decimal ExchangeValue { get; set; }
        decimal PercentExchange { get; set; }
        decimal ProfitLossNotRealizedValue { get; set; }
        decimal PercentProfitLossN { get; set; }
    }

    public interface ISummaryBeginningYear
    {       
        decimal ValuePrice { get; set; }
        decimal PercentPrice { get; set; }
        decimal ExchangeValue { get; set; }
        decimal PercentExchange { get; set; }
        decimal ProfitLossNotRealizedValue { get; set; }
        decimal PercentProfitLossN { get; set; }
    }

    public interface ISummaryPurchase
    {
        decimal ValuePrice { get; set; }
        decimal PercentPrice { get; set; }
        decimal ExchangeValue { get; set; }
        decimal? PercentExchange { get; set; }
        decimal ProfitLossNotRealizedValue { get; set; }
        decimal PercentProfitLossN { get; set; }
    }

}
