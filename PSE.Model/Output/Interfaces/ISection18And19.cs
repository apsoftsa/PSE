namespace PSE.Model.Output.Interfaces
{

    public interface IAlternativeProductDetail
    {

        string Description { get; set; }

        string ValorNumber { get; set; }

        string Isin { get; set; }

        string NominalAmmount { get; set; }

        string Currency { get; set; }

        string PurchasePrice { get; set; }

        string PriceBeginningYear { get; set; }

        string CurrentPriceFromPurchase { get; set; }

        string CurrentPriceFromYTD { get; set; }

        string ExchangeRateImpactPurchase { get; set; }

        string ExchangeRateImpactYTD { get; set; }

        string PercentPerformancePurchase { get; set; }

        string PercentPerformanceYTD { get; set; }

        string PercentAssets { get; set; }

    }

    public interface IAlternativeProducts
    {

        IList<IAlternativeProductDetail> DerivativesOnSecurities { get; set; }

        IList<IAlternativeProductDetail> DerivativesOnMetals { get; set; }

        IList<IAlternativeProductDetail> DerivativesFutures { get; set; }

        IList<IAlternativeProductDetail> Different { get; set; }

    }

    public interface ISection18And19Content
    {

        IList<IAlternativeProducts> AlternativeProducts { get; set; }

    }

}
