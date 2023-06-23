namespace PSE.Model.Output.Interfaces
{

    public interface IBondsWithMaturityGreatherThanFiveYears
    {

        string Description { get; set; }

        string ValorNumber { get; set; }

        string Isin { get; set; }

        string NominalAmount { get; set; }

        string Currency { get; set; }

        string SPRating { get; set; }

        string MsciEsg { get; set; }

        string PurchasePrice { get; set; }

        string PriceBeginningYear { get; set; }

        string CurrentPriceFromPurchase { get; set; }

        string CurrentPriceFromYTD { get; set; }

        string ExchangeRateImpactPurchase { get; set; }

        string ExchangeRateImpactYTD { get; set; }

        string PerformancePurchase { get; set; }

        string PercentperformanceYTD { get; set; }

        string PercentAssets { get; set; }

    }

    public interface ISection15Content
    {

        IList<IBondsWithMaturityGreatherThanFiveYears> BondsWithMatGreatThanFiveYears { get; set; }

    }

}
