namespace PSE.Model.Output.Interfaces
{

    public interface IInvestmentCurrency
    {

        string Currency { get; set; }

        decimal? Amount { get; set; }

        decimal? Exchange { get; set; }

        decimal? MarketValueReportingCurrency { get; set; }

        decimal? PercentAsset { get; set; }

    }

    public interface IChartInvestmentCurrency
    {       

        string Currency { get; set; }

        decimal? PercentAsset { get; set; }

    }

    public interface ISubSection6000Content
    {

        string Name { get; set; }

        IList<IInvestmentCurrency> Content { get; set; }

    }

    public interface ISubSection6010Content
    {

        string Name { get; set; }

        IList<IChartInvestmentCurrency> Content { get; set; }

    }

    public interface ISection060Content
    {

        ISubSection6000Content? SubSection6000 { get; set; }

        ISubSection6010Content? SubSection6010 { get; set; }

    }

}
