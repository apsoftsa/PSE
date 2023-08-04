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

        [JsonProperty("amount1", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? AmountLoss { get; set; }

        [JsonProperty("currency1", NullValueHandling = NullValueHandling.Ignore)]
        public string? CurrencyLoss { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Change { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Amount2 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Currency2 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? ExpirationDate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? CurrentExchangeRate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ProfitlossReportingCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PercentAssets { get; set; }

        public ProfitLossOperation()
        {
            AmountLoss = null;
            CurrencyLoss = null;
            Change = null;
            Amount2 = null;
            Currency2 = null;
            ExpirationDate = null;
            CurrentExchangeRate = null;
            ProfitlossReportingCurrency = null;
            PercentAssets = null;
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
