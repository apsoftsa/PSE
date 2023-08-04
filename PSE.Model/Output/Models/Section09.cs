using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ShortTermInvestment : IShortTermInvestment
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Description { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? DescriptionExtra { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? ValorNumber { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Isin { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? NominalAmount { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Currency { get; set; }

        [JsonProperty("spRating", NullValueHandling = NullValueHandling.Ignore)]
        public string? SPRating { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? MsciEsg { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PurchasePrice { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PriceBeginningYear { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? CurrentPrice { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ExchangeRateImpactPurchase { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ExchangeRateImpactYTD { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PerformancePurchase { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PercentPerformancePurchase { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PerformanceYTD { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PercentPerformanceYTD { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? PercentAsset { get; set; }
                      
        public ShortTermInvestment()
        {
            Description = null;
            DescriptionExtra = null;
            Isin = null;
            ValorNumber = null;
            NominalAmount = null;
            Currency = null;
            SPRating = null;
            MsciEsg = null;
            PurchasePrice = null;
            PriceBeginningYear = null;
            CurrentPrice = null;
            ExchangeRateImpactPurchase = null;
            ExchangeRateImpactYTD = null;
            PerformancePurchase = null;
            PercentPerformancePurchase = null;
            PerformanceYTD = null;
            PercentPerformanceYTD = null;
            PercentAsset = null;
        }

        public ShortTermInvestment(IShortTermInvestment source)
        {
            Description = source.Description;
            Isin = source.Isin;
            ValorNumber = source.ValorNumber;
            NominalAmount = source.NominalAmount;
            Currency = source.Currency;
            SPRating = source.SPRating;
            MsciEsg = source.MsciEsg;
            PurchasePrice = source.PurchasePrice;
            DescriptionExtra = source.DescriptionExtra;
            PriceBeginningYear = source.PriceBeginningYear;
            CurrentPrice = source.CurrentPrice;
            ExchangeRateImpactPurchase = source.ExchangeRateImpactPurchase;
            ExchangeRateImpactYTD = source.ExchangeRateImpactYTD;
            PerformancePurchase = source.PerformancePurchase;
            PercentPerformancePurchase = source.PercentPerformancePurchase;
            PerformanceYTD = source.PerformanceYTD;
            PercentPerformanceYTD = source.PercentPerformanceYTD;
            PercentAsset = source.PercentAsset;
        }

    }    

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section9Content : ISection9Content
    {

        [JsonProperty(propertyName: "investment")]
        public IList<IShortTermInvestment> Investments { get; set; }

        public Section9Content()
        {
            Investments = new List<IShortTermInvestment>();
        }

        public Section9Content(ISection9Content source)
        {
            Investments = new List<IShortTermInvestment>();
            if (source != null)
            {
                if (source.Investments != null && source.Investments.Any())
                {
                    foreach (IShortTermInvestment _shortTermInv in source.Investments)
                    {
                        Investments.Add(new ShortTermInvestment(_shortTermInv));
                    }
                }
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section9 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection9Content Content { get; set; }

        public Section9() : base(OUTPUT_SECTION9_CODE)
        {
            Content = new Section9Content();
        }

        public Section9(Section9 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section9Content(source.Content);
            else
                Content = new Section9Content();
        }

    }

}
