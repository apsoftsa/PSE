using Newtonsoft.Json;
using PSE.Model.Output.Interfaces;

[Serializable]
[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public abstract class BondsBase : IBondsBase
{

    public string Description { get; set; }

    public string ValorNumber { get; set; }

    public string Isin { get; set; }

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

    public BondsBase()
    {
        Description = string.Empty;
        ValorNumber = string.Empty;
        Isin = string.Empty;
        Currency = string.Empty;
        PercentCoupon = string.Empty;
        PercentYTM = string.Empty;
        Expiration = string.Empty;
        SPRating = string.Empty;
        MsciEsg = string.Empty;
        PurchasePrice = string.Empty;
        PriceAtTheBeginningOfTheYear = string.Empty;
        CurrentPrice = string.Empty;
        PercentImpactChangeFromPurchase = string.Empty;
        PercentImpactChangeYTD = string.Empty;
        PercentPerformancefromPurchase = string.Empty;
        PercentPerformanceYTD = string.Empty;
        PercentAssets = string.Empty;
    }

    public BondsBase(IBondsBase source)
    {
        Description = source.Description;
        ValorNumber = source.ValorNumber;
        Isin = source.Isin;
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
