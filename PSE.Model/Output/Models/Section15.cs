using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class BondsWithMaturityGreatherThanFiveYears : IBondsWithMaturityGreatherThanFiveYears
    {

        public string Description { get; set; }

        public string ValorNumber { get; set; }

        public string Isin { get; set; }

        public string NominalAmount { get; set; }

        public string Currency { get; set; }

        [JsonProperty(propertyName: "s&pRating")]
        public string SPRating { get; set; }

        public string MsciEsg { get; set; }

        public string PurchasePrice { get; set; }

        public string PriceBeginningYear { get; set; }

        public string CurrentPriceFromPurchase { get; set; }

        public string CurrentPriceFromYTD { get; set; }

        public string ExchangeRateImpactPurchase { get; set; }

        public string ExchangeRateImpactYTD { get; set; }

        public string PerformancePurchase { get; set; }

        public string PercentperformanceYTD { get; set; }

        public string PercentAssets { get; set; }


        public BondsWithMaturityGreatherThanFiveYears()
        {
            Description = string.Empty;
            ValorNumber = string.Empty;
            Isin = string.Empty;
            NominalAmount = string.Empty;
            Currency = string.Empty;
            SPRating = string.Empty;
            MsciEsg = string.Empty;
            PurchasePrice = string.Empty;
            PriceBeginningYear = string.Empty;            
            CurrentPriceFromPurchase = string.Empty;
            CurrentPriceFromYTD = string.Empty;
            ExchangeRateImpactPurchase = string.Empty;
            ExchangeRateImpactYTD = string.Empty;
            PerformancePurchase = string.Empty;
            PercentperformanceYTD = string.Empty;
            PercentAssets = string.Empty;
        }

        public BondsWithMaturityGreatherThanFiveYears(IBondsWithMaturityGreatherThanFiveYears source)
        {
            Description = source.Description;
            ValorNumber = source.ValorNumber;
            Isin = source.Isin;
            NominalAmount = source.NominalAmount;
            Currency = source.Currency;
            SPRating = source.SPRating;
            MsciEsg = source.MsciEsg;
            PurchasePrice = source.PurchasePrice;
            PriceBeginningYear = source.PriceBeginningYear;
            CurrentPriceFromPurchase = source.CurrentPriceFromPurchase;
            CurrentPriceFromYTD = source.CurrentPriceFromYTD;
            ExchangeRateImpactPurchase = source.ExchangeRateImpactPurchase;
            ExchangeRateImpactYTD = source.ExchangeRateImpactYTD;
            PerformancePurchase = source.PerformancePurchase;
            PercentperformanceYTD = source.PercentperformanceYTD;
            PercentAssets = source.PercentAssets;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section15Content : ISection15Content
    {

        [JsonProperty(propertyName: "bondsWithMaturityGreatherThanFiveYears")]
        public IList<IBondsWithMaturityGreatherThanFiveYears> BondsWithMatGreatThanFiveYears { get; set; }

        public Section15Content()
        {
            BondsWithMatGreatThanFiveYears = new List<IBondsWithMaturityGreatherThanFiveYears>();
        }

        public Section15Content(ISection15Content source)
        {
            BondsWithMatGreatThanFiveYears = new List<IBondsWithMaturityGreatherThanFiveYears>();
            if (source != null)
            {
                if (source.BondsWithMatGreatThanFiveYears != null && source.BondsWithMatGreatThanFiveYears.Any())
                {
                    foreach (IBondsWithMaturityGreatherThanFiveYears _bwmgt5y in source.BondsWithMatGreatThanFiveYears)
                    {
                        BondsWithMatGreatThanFiveYears.Add(new BondsWithMaturityGreatherThanFiveYears(_bwmgt5y));
                    }
                }
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section15 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection15Content Content { get; set; }

        public Section15() : base(OUTPUT_SECTION15_CODE)
        {
            Content = new Section15Content();
        }

        public Section15(Section15 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section15Content(source.Content);
            else
                Content = new Section15Content();
        }

    }

}
