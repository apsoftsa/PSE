namespace PSE.Model.Output.Interfaces
{

    public interface ISubAsset
    {

        string TypeInvestment { get; set; }

        string MarketValueReportingCurrency { get; set; }

        string PercentInvestment { get; set; }

    }

    public interface IAsset
    {        

        string AssetClass { get; set; }

        string MarketValueReportingCurrency { get; set; }

        string PercentInvestment { get; set; }

        IList<ISubAsset> SubAssets { get; set; }

    }

    public interface IChartAsset
    {

        string AssetClass { get; set; }

        string PercentInvestment { get; set; }

    }


    public interface ISection6Content
    {

        IList<IAsset> Assets { get; set; }

        IList<IChartAsset> ChartAssets { get; set; }

    }

}
