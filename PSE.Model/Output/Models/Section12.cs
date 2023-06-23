using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class BondsMaturingLessThan5Years : IBondsMaturingLessThan5Years
    {

        public string Description { get; set; }

        public string ValorNumber { get; set; }

        public string Isin { get; set; }

        [JsonProperty(propertyName: "amount Nominal")]
        public string AmountNominal { get; set; }

        public string Currency { get; set; }

        public string PercentCoupon { get; set; }

        public string PercentYTM { get; set; }

        public string Expiration { get; set; }

        [JsonProperty(propertyName: "s&pRating")]
        public string SPRating { get; set; }

        public string MsciEsg { get; set; }

        public string PurchasePrice { get; set; }

        public string PriceAtTheBeginningOfTheYear { get; set; }

        public string CurrentPrice { get; set; }

        public string PercentImpactChangeFromPurchase { get; set; }

        public string PercentImpactChangeYTD { get; set; }

        public string PercentPerformancefromPurchase { get; set; }

        public string PercentPerformanceYTD { get; set; }

        public string PercentAssets { get; set; }

        public BondsMaturingLessThan5Years()
        {
            Description= string.Empty;
            ValorNumber= string.Empty;
            Isin= string.Empty;
            AmountNominal= string.Empty;
            Currency= string.Empty;
            PercentCoupon= string.Empty;
            PercentYTM= string.Empty;
            Expiration= string.Empty;
            SPRating= string.Empty;
            MsciEsg= string.Empty;
            PurchasePrice= string.Empty;
            PriceAtTheBeginningOfTheYear= string.Empty;
            CurrentPrice= string.Empty;
            PercentImpactChangeFromPurchase= string.Empty;
            PercentImpactChangeYTD= string.Empty;
            PercentPerformancefromPurchase= string.Empty;
            PercentPerformanceYTD= string.Empty;
            PercentAssets= string.Empty;
        }

        public BondsMaturingLessThan5Years(IBondsMaturingLessThan5Years source)
        {
            Description = source.Description;
            ValorNumber = source.ValorNumber;
            Isin = source.Isin;
            AmountNominal = source.AmountNominal;
            Currency = source.Currency;
            PercentCoupon = source.PercentCoupon;
            PercentYTM = source.PercentYTM;
            Expiration = source.Expiration;
            SPRating = source.SPRating;
            MsciEsg = source.MsciEsg;
            PurchasePrice = source.PurchasePrice;
            PriceAtTheBeginningOfTheYear = source.PriceAtTheBeginningOfTheYear;
            CurrentPrice = source.CurrentPrice;
            PercentImpactChangeFromPurchase = source.PercentImpactChangeFromPurchase;
            PercentImpactChangeYTD = source.PercentImpactChangeYTD;
            PercentPerformancefromPurchase = source.PercentPerformancefromPurchase;
            PercentPerformanceYTD = source.PercentPerformanceYTD;
            PercentAssets = source.PercentAssets;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section12Content : ISection12Content
    {

        [JsonProperty(propertyName: "bondsMaturing<5years")]
        public IList<IBondsMaturingLessThan5Years> BondsMaturingLessThan5Years { get; set; }

        public Section12Content()
        {
            BondsMaturingLessThan5Years = new List<IBondsMaturingLessThan5Years>();
        }

        public Section12Content(ISection12Content source)
        {
            BondsMaturingLessThan5Years = new List<IBondsMaturingLessThan5Years>();
            if (source != null)
            {
                if (source.BondsMaturingLessThan5Years != null && source.BondsMaturingLessThan5Years.Any())
                {
                    foreach (IBondsMaturingLessThan5Years _bmlt5y in source.BondsMaturingLessThan5Years)
                    {
                        BondsMaturingLessThan5Years.Add(new BondsMaturingLessThan5Years(_bmlt5y));
                    }
                }
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section12 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection12Content Content { get; set; }

        public Section12() : base(OUTPUT_SECTION12_CODE)
        {
            Content = new Section12Content();
        }

        public Section12(Section12 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section12Content(source.Content);
            else
                Content = new Section12Content();
        }

    }

}
