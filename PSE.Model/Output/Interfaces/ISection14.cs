namespace PSE.Model.Output.Interfaces
{

    public interface IObligationsWithMaturityGreatherThanFiveYears
    {

        string Description { get; set; }

        string ValorNumber { get; set; }

        string Isin { get; set; }

        string NominalAmount { get; set; }

        string Currency { get; set; }

        string PercentCoupon { get; set; }

        string PercentYTM { get; set; }

        string Expiration { get; set; }

        string SPRating { get; set; }

        string MsciEsg { get; set; }

        string PurchasePrice { get; set; }

        string PriceBeginningYear { get; set; }

        string CurrentPriceFromPurchase { get; set; }

        string CurrentPriceFromYTD { get; set; }

        string ExchangeRateImpactPurchase { get; set; }

        string ExchangeRateImpactYTD { get; set; }

        string PerformancePurchase { get; set; }

        string PerformanceYTD { get; set; }

        string PercentAssets { get; set; }

    }

    public interface ISection14Content
    {

        IList<IObligationsWithMaturityGreatherThanFiveYears> ObligationsWithMaturityGreatherThanFiveYears { get; set; }

    }

}
