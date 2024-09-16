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
    public class DetailSummary : IDetailSummary
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? ValueDate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ValuePrice { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PercentPrice { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ExchangeValue { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PercentExchange { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ProfitLossNotRealizedValue { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PercentProfitLossN { get; set; }

        public DetailSummary()
        {
            ValueDate = null;
            ValuePrice = null;
            PercentPrice = null;
            ExchangeValue = null;
            PercentExchange = null;
            ProfitLossNotRealizedValue = null;
            PercentProfitLossN = null;
        }

        public DetailSummary(IDetailSummary source)
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

}
