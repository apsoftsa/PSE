namespace PSE.Model.Output.Interfaces
{

    public interface IAlternativeProductDetail
    {

        string? Description { get; set; }

        string? DescriptionExtra { get; set; }

        long? ValorNumber { get; set; }

        string? Isin { get; set; }

        decimal? NominalAmmount { get; set; }

        string? UnderlyingDescription { get; set; }

        string? Currency { get; set; }

        decimal? PurchasePrice { get; set; }

        decimal? PriceBeginningYear { get; set; }

        decimal? CurrentPrice { get; set; }

        decimal? ExchangeRateImpactPurchase { get; set; }

        decimal? ExchangeRateImpactYTD { get; set; }

        decimal? PerformancePurchase { get; set; }

        decimal? PercentPerformancePurchase { get; set; }

        decimal? PerformanceYTD { get; set; }

        decimal? PercentPerformanceYTD { get; set; }

        decimal? PercentAsset { get; set; }

    }

    public interface IAlternativeProducts
    {

        IList<IAlternativeProductDetail> DerivativesOnSecurities { get; set; }

        IList<IAlternativeProductDetail> DerivativesOnMetals { get; set; }

        IList<IAlternativeProductDetail> DerivativesFutures { get; set; }

        IList<IAlternativeProductDetail> Different { get; set; }

        IList<IAlternativeProductDetail> DifferentExtra { get; set; }

    }

    public interface ISection18And19Content
    {

        IList<IAlternativeProducts> AlternativeProducts { get; set; }

    }

}
