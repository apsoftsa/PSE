namespace PSE.Model.Output.Interfaces
{

    public interface IInvestmentAsset
    {        

        string? AssetClass { get; set; }

        decimal? MarketValueReportingCurrencyT { get; set; }

        decimal? PercentInvestmentT { get; set; }

        string? TypeInvestment { get; set; }

        decimal? MarketValueReportingCurrency { get; set; }

        decimal? PercentInvestment { get; set; }

    }

    public interface IAssetClassChart
    {

        string? AssetClass { get; set; }

        decimal? PercentInvestment { get; set; }

    }

    public interface ISubSection4000Content
    {

        string Name { get; set; }

        IList<IInvestmentAsset> Content { get; set; }

    }

    public interface ISubSection4010Content
    {

        string Name { get; set; }

        IList<IAssetClassChart> Content { get; set; }

    }

    public interface ISection040Content
    {

        ISubSection4000Content SubSection4000 { get; set; }

        ISubSection4010Content SubSection4010 { get; set; }

    }

}
