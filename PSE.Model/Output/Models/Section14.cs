using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ObligationsWithMaturityGreatherThanFiveYears : IObligationsWithMaturityGreatherThanFiveYears
    {

        public string Description { get; set; }

        public string ValorNumber { get; set; }

        public string Isin { get; set; }

        public string NominalAmount { get; set; }

        public string Currency { get; set; }

        public string PercentCoupon { get; set; }

        public string PercentYTM { get; set; }

        public string Expiration { get; set; }

        [JsonProperty(propertyName: "s&pRatings")]
        public string SPRating { get; set; }

        public string MsciEsg { get; set; }

        public string PurchasePrice { get; set; }

        public string PriceBeginningYear { get; set; }

        public string CurrentPriceFromPurchase { get; set; }

        public string CurrentPriceFromYTD { get; set; }

        public string ExchangeRateImpactPurchase { get; set; }

        public string ExchangeRateImpactYTD { get; set; }

        public string PerformancePurchase { get; set; }

        public string PerformanceYTD { get; set; }

        public string PercentAssets { get; set; }

        public ObligationsWithMaturityGreatherThanFiveYears()
        {
            Description = string.Empty;
            ValorNumber = string.Empty;
            Isin = string.Empty;
            NominalAmount = string.Empty;
            Currency = string.Empty;
            PercentCoupon = string.Empty;
            PercentYTM = string.Empty;
            Expiration = string.Empty;
            SPRating = string.Empty;
            MsciEsg = string.Empty;
            PurchasePrice = string.Empty;
            PriceBeginningYear = string.Empty;
            CurrentPriceFromPurchase = string.Empty;
            CurrentPriceFromYTD = string.Empty;
            ExchangeRateImpactPurchase = string.Empty;
            ExchangeRateImpactYTD = string.Empty;
            PerformancePurchase = string.Empty;
            PerformanceYTD = string.Empty;
            PercentAssets = string.Empty;
        }

        public ObligationsWithMaturityGreatherThanFiveYears(IObligationsWithMaturityGreatherThanFiveYears source)
        {
            Description = source.Description;
            ValorNumber = source.ValorNumber;
            Isin = source.Isin;
            NominalAmount = source.NominalAmount;
            Currency = source.Currency;
            PercentCoupon = source.PercentCoupon;
            PercentYTM = source.PercentYTM;
            Expiration = source.Expiration;
            SPRating = source.SPRating;
            MsciEsg = source.MsciEsg;
            PurchasePrice = source.PurchasePrice;
            PriceBeginningYear = source.PriceBeginningYear;
            CurrentPriceFromPurchase = source.CurrentPriceFromPurchase;
            CurrentPriceFromYTD = source.CurrentPriceFromYTD;
            ExchangeRateImpactPurchase = source.ExchangeRateImpactPurchase;
            ExchangeRateImpactYTD = source.ExchangeRateImpactYTD;
            PerformancePurchase = source.PerformancePurchase;
            PerformanceYTD = source.PerformanceYTD;
            PercentAssets = source.PercentAssets;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section14Content : ISection14Content
    {

        [JsonProperty(propertyName: "obligationsWithMaturityGreatherThanFiveYears")]
        public IList<IObligationsWithMaturityGreatherThanFiveYears> ObligationsWithMaturityGreatherThanFiveYears { get; set; }

        public Section14Content()
        {
            ObligationsWithMaturityGreatherThanFiveYears = new List<IObligationsWithMaturityGreatherThanFiveYears>();
        }

        public Section14Content(ISection14Content source)
        {
            ObligationsWithMaturityGreatherThanFiveYears = new List<IObligationsWithMaturityGreatherThanFiveYears>();
            if (source != null)
            {
                if (source.ObligationsWithMaturityGreatherThanFiveYears != null && source.ObligationsWithMaturityGreatherThanFiveYears.Any())
                {
                    foreach (IObligationsWithMaturityGreatherThanFiveYears _owmgt5y in source.ObligationsWithMaturityGreatherThanFiveYears)
                    {
                        ObligationsWithMaturityGreatherThanFiveYears.Add(new ObligationsWithMaturityGreatherThanFiveYears(_owmgt5y));
                    }
                }
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section14 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection14Content Content { get; set; }

        public Section14() : base(OUTPUT_SECTION14_CODE)
        {
            Content = new Section14Content();
        }

        public Section14(Section14 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section14Content(source.Content);
            else
                Content = new Section14Content();
        }

    }

}
