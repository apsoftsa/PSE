namespace PSE.Model.Output.Interfaces
{

    public interface IKeyInformation
    {

        string Customer { get; set; }

        string CustomerID { get; set; }

        string Portfolio { get; set; }        

        int? RiskProfile { get; set; }

        string EsgProfile { get; set; }

    }

    public interface IAssetExtract
    {

        string Entry { get; set; }

        decimal? MarketValueReportingCurrency { get; set; }

        string? AssetType { get; set; }

        decimal? MarketValueReportingCurrencyContr { get; set; }

    }

    public interface IDividendInterest
    {

        string Entry { get; set; }

        decimal MarketValueReportingCurrencyT { get; set; }

        string? AssetType { get; set; }

        decimal? MarketValueReportingCurrency { get; set; }

    }

    public interface IFooterInformation
    {

        string Note { get; set; }

    }

    public interface ISubSection1000Content
    {

        string Name { get; set; }

        IList<IKeyInformation> Content { get; set; }

    }

    public interface ISubSection1010Content
    {

        string Name { get; set; }

        IList<IAssetExtract> Content { get; set; }

    }

    public interface ISubSection1011Content
    {

        string Name { get; set; }

        IList<IDividendInterest> Content { get; set; }

        IList<IFooterInformation> Notes { get; set; }

    }

    public interface ISection010Content
    {

        ISubSection1000Content? SubSection1000 { get; set; }

        ISubSection1010Content? SubSection1010 { get; set; }

        ISubSection1011Content? SubSection1011 { get; set; }

    }

}
