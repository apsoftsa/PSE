namespace PSE.Model.Output.Interfaces
{

    public interface IKeyInformation
    {

        string? CustomerName { get; set; }

        string? CustomerNumber { get; set; }

        string? Portfolio { get; set; }

        string? Service { get; set; }

        string? RiskProfile { get; set; }

        decimal? PercentWeightedPerformance { get; set; }

    }

    public interface IAssetExtract
    {

        string? AssetClass { get; set; }

        decimal? MarketValueReportingCurrencyT { get; set; }

        string? AssetType { get; set; }

        decimal? MarketValueReportingCurrency { get; set; }

    }

    public interface IFooterInformation
    {

        string? Footer1 { get; set; }

        string? Footer2 { get; set; }

    }

    public interface ISection3Content
    {

        IList<IKeyInformation> KeysInformation { get; set; }

        IList<IAssetExtract> AssetsExtract { get; set; }

        IList<IAssetExtract> DividendsInterests { get; set; }

        IList<IFooterInformation> FooterInformation { get; set; }

    }

}
