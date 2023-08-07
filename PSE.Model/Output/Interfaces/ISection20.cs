namespace PSE.Model.Output.Interfaces
{

    public interface IMetalPhysicalMetalAccount
    {

        string? Account { get; set; }

        decimal? Amount { get; set; }

        decimal? CostPrice { get; set; }

        decimal? PurchasingCourse { get; set; }

        decimal? PercentDifference { get; set; }

        decimal? CurrentBalance { get; set; }

        decimal? MarketValueReportingCurrency { get; set; }

        decimal? PercentAsset { get; set; }

    }

    public interface ISection20Content
    {

        IList<IMetalPhysicalMetalAccount> MetalPhysicalMetalAccounts { get; set; }

    }

}