namespace PSE.Model.Output.Interfaces
{

    public interface IBondsMaturingLessThan5Years
    {

        string Description { get; set; }

        string ValorNumber { get; set; }

        string Isin { get; set; }

        string AmountNominal { get; set; }

        string Currency { get; set; }

        string PercentCoupon { get; set; }

        string PercentYTM { get; set; }

        string Expiration { get; set; }

        string SPRating { get; set; }

        string MsciEsg { get; set; }

        string PurchasePrice { get; set; }

        string PriceAtTheBeginningOfTheYear { get; set; }

        string CurrentPrice { get; set; }

        string PercentImpactChangeFromPurchase { get; set; }

        string PercentImpactChangeYTD { get; set; }

        string PercentPerformancefromPurchase { get; set; }

        string PercentPerformanceYTD { get; set; }

        string PercentAssets { get; set; }

    }

    public interface ISection12Content
    {

        IList<IBondsMaturingLessThan5Years> BondsMaturingLessThan5Years { get; set; }

    }

}
