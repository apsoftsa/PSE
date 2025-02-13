using Newtonsoft.Json;
using PSE.Model.Output.Interfaces;

namespace PSE.Model.Output.Models
{
    public class Settled : ISettled
    {

        public string Date { get; set; }

        public string Time { get; set; }

        public Settled()
        {
            Date = string.Empty;
            Time = string.Empty;
        }

        public Settled(ISettled source)
        {
            Date = source.Date;
            Time = source.Time;
        }

        public Settled(string date, string time)
        {
            Date = date;
            Time = time;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class SummaryTo : ISummaryTo
    {

        public string ValueDate { get; set; }

        public decimal? ValuePrice { get; set; }

        public decimal? PercentPrice { get; set; }

        public decimal? ExchangeValue { get; set; }

        public decimal? PercentExchange { get; set; }

        public decimal? ProfitLossNotRealizedValue { get; set; }

        public decimal? PercentProfitLossN { get; set; }

        public SummaryTo()
        {
            ValueDate = string.Empty;
            ValuePrice = 0;
            PercentPrice = 0;
            ExchangeValue = 0;
            PercentExchange = 0;
            ProfitLossNotRealizedValue = 0;
            PercentProfitLossN = 0;
        }

        public SummaryTo(ISummaryTo source)
        {
            ValueDate = source.ValueDate;
            ValuePrice = source.ValuePrice;
            PercentPrice = source.PercentPrice;
            ExchangeValue = source.ExchangeValue;
            PercentExchange = source.PercentExchange;
            ProfitLossNotRealizedValue = source.ProfitLossNotRealizedValue;
            PercentProfitLossN = source.PercentProfitLossN;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class SummaryBeginningYear : ISummaryBeginningYear
    {

        public decimal? ValuePrice { get; set; }

        public decimal? PercentPrice { get; set; }

        public decimal? ExchangeValue { get; set; }

        public decimal? PercentExchange { get; set; }

        public decimal? ProfitLossNotRealizedValue { get; set; }

        public decimal? PercentProfitLossN { get; set; }

        public SummaryBeginningYear()
        {
            ValuePrice = 0;
            PercentPrice = 0;
            ExchangeValue = 0;
            PercentExchange = 0;
            ProfitLossNotRealizedValue = 0;
            PercentProfitLossN = 0;
        }

        public SummaryBeginningYear(ISummaryBeginningYear source)
        {
            ValuePrice = source.ValuePrice;
            PercentPrice = source.PercentPrice;
            ExchangeValue = source.ExchangeValue;
            PercentExchange = source.PercentExchange;
            ProfitLossNotRealizedValue = source.ProfitLossNotRealizedValue;
            PercentProfitLossN = source.PercentProfitLossN;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class SummaryPurchase : ISummaryPurchase
    {

        public decimal? ValuePrice { get; set; }

        public decimal? PercentPrice { get; set; }

        public decimal? ExchangeValue { get; set; }

        public decimal? PercentExchange { get; set; }

        public decimal? ProfitLossNotRealizedValue { get; set; }

        public decimal? PercentProfitLossN { get; set; }

        public SummaryPurchase()
        {
            ValuePrice = 0;
            PercentPrice = 0;
            ExchangeValue = 0;
            PercentExchange = 0;
            ProfitLossNotRealizedValue = 0;
            PercentProfitLossN = 0;
        }

        public SummaryPurchase(ISummaryPurchase source)
        {
            ValuePrice = source.ValuePrice;
            PercentPrice = source.PercentPrice;
            ExchangeValue = source.ExchangeValue;
            PercentExchange = source.PercentExchange;
            ProfitLossNotRealizedValue = source.ProfitLossNotRealizedValue;
            PercentProfitLossN = source.PercentProfitLossN;
        }

    }

}
