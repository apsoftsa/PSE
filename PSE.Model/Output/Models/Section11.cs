using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Output.Models
{
   
    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ProfitLossOperation : IProfitLossOperation
    {

        [JsonProperty("amountl", NullValueHandling = NullValueHandling.Ignore)]
        public string AmountLoss { get; set; }

        [JsonProperty("currencyl", NullValueHandling = NullValueHandling.Ignore)]
        public string CurrencyLoss { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Change { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Amount2 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Currency2 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ExpirationDate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CurrentExchangeRate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ProfitlossReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PercentAssets { get; set; }

        public ProfitLossOperation()
        {
            AmountLoss = string.Empty;
            CurrencyLoss = string.Empty;
            Change = string.Empty;
            Amount2 = string.Empty;
            Currency2 = string.Empty;
            ExpirationDate = string.Empty;
            CurrentExchangeRate = string.Empty;
            ProfitlossReportingCurrency = string.Empty;
            PercentAssets = string.Empty;
        }

        public ProfitLossOperation(IProfitLossOperation source)
        {
            AmountLoss = source.AmountLoss;
            CurrencyLoss = source.CurrencyLoss;
            Change = source.Change;
            Amount2 = source.Amount2;
            Currency2 = source.Currency2;
            ExpirationDate = source.ExpirationDate;
            CurrentExchangeRate = source.CurrentExchangeRate;
            ProfitlossReportingCurrency = source.ProfitlossReportingCurrency;
            PercentAssets = source.PercentAssets;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section11Content : ISection11Content
    {

        public IList<IProfitLossOperation> Operations { get; set; }

        public Section11Content()
        {
            Operations = new List<IProfitLossOperation>();
        }

        public Section11Content(ISection11Content source)
        {
            Operations = new List<IProfitLossOperation>();
            if (source != null)
            {
                if (source.Operations != null && source.Operations.Any())
                {
                    foreach (IProfitLossOperation _profitLoss in source.Operations)
                    {
                        Operations.Add(new ProfitLossOperation(_profitLoss));
                    }
                }
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section11 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection11Content Content { get; set; }

        public Section11() : base(OUTPUT_SECTION11_CODE)
        {
            Content = new Section11Content();
        }

        public Section11(Section11 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section11Content(source.Content);
            else
                Content = new Section11Content();
        }

    }

}
