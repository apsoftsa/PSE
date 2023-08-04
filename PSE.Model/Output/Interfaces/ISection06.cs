namespace PSE.Model.Output.Interfaces
{

    public interface IAsset
    {        

        string? AssetClass { get; set; }

        decimal? MarketValueReportingCurrencyT { get; set; }

        decimal? PercentInvestmentT { get; set; }

        string? TypeInvestment { get; set; }

        int? MarketValueReportingCurrency { get; set; }

        int? PercentInvestment { get; set; }

    }

    public interface IChartAsset
    {

        string? AssetClass { get; set; }

        int? PercentInvestment { get; set; }

    }


    public interface ISection6Content
    {

        IList<IAsset> Assets { get; set; }

        IList<IChartAsset> ChartAssets { get; set; }

    }

}
