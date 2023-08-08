using Newtonsoft.Json;
using PSE.Model.Output.Interfaces;

[Serializable]
[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public abstract class ObligationsBase : IObligationsBase
{

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string? Description { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public long? ValorNumber { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string? Isin { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public decimal? NominalAmount { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string? Currency { get; set; }

    [JsonProperty(propertyName: "spRating", NullValueHandling = NullValueHandling.Ignore)]
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

    public ObligationsBase()
    {
        Description = null;
        ValorNumber = null;
        Isin = null;
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

    public ObligationsBase(IObligationsBase source)
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
public abstract class BondsBase : IBondsBase
{

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string? Description { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public long? ValorNumber { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string? Isin { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public decimal? NominalAmount { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string? Currency { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public int? PercentCoupon { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public decimal? PercentYTM { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string? Expiration { get; set; }

    [JsonProperty(propertyName: "spRating", NullValueHandling = NullValueHandling.Ignore)]
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

    public BondsBase() 
    {
        PercentCoupon = null;
        PercentYTM = null;
        Expiration = null;
        Description = null;
        ValorNumber = null;
        Isin = null;
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

    public BondsBase(IBondsBase source)   
    {
        PercentCoupon = source.PercentCoupon;
        PercentYTM = source.PercentYTM;
        Expiration = source.Expiration;
        Description = source.Description;
        ValorNumber = source.ValorNumber;
        Isin = source.Isin;
        NominalAmount = source.NominalAmount;
        Currency = source.Currency;
        SPRating = source.SPRating;
        MsciEsg = source.MsciEsg;
        PurchasePrice = source.PurchasePrice;
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
