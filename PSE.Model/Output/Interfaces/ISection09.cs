namespace PSE.Model.Output.Interfaces
{

    public interface IShortTermInvestment
    {

        string Description { get; set; }

        string Isin { get; set; }

        string ValorNumber { get; set; }

        string NominalAmount { get; set; }

        string Currency { get; set; }

        string SAndPRating { get; set; }

        string MsciEsg { get; set; }

        string PurchasePrice { get; set; }

        string CurrentPriceToPurchase { get; set; }

        string PercentImpactChangeToPurchase { get; set; }

        string PriceAtTheBeginningOfTheYear { get; set; }

        string CurrentPriceYTD { get; set; }

        string PercentImpactChangeYTD { get; set; }

        string Performance { get; set; }

        string PercentPerformance { get; set; }

        string PercentAssets { get; set; }

    }

    public interface ISection9Content
    {

        IList<IShortTermInvestment> Investments { get; set; }

    }

}
