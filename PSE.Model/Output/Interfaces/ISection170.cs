namespace PSE.Model.Output.Interfaces
{
    public interface IShareByCountry
    {
        string? Country { get; set; }
        decimal? MarketValueReportingCurrency { get; set; }
        decimal? PercentShares { get; set; }
    }

    public interface IShareByNation
    {
        string? Nation { get; set; }
        IList<IShareByCountry>? Content { get; set; }
        decimal? MarketValueReportingCurrency { get; set; }
        decimal? PercentShares { get; set; }
    }

    public interface ITotalShares
    {
        decimal? TotalSharesValue { get; set; }
        decimal? TotalMarketValueReportingCurrency { get; set; }
        decimal? TotalPercentShares { get; set; }
    }

    public interface ISubSection17000
    {
        string? Name { get; set; }
        IList<IShareByNation>? Content { get; set; }
        ITotalShares? TotalSharesData { get; set; }
    }

    public interface IShareByNationChart
    {
        string? Nation { get; set; }
        decimal? PercentShares { get; set; }
    }

    public interface ISubSection17010
    {
        string? Name { get; set; }
        IList<IShareByNationChart>? Content { get; set; }
    }

    public interface ISection170Content
    {
        ISubSection17000? SubSection17000 { get; set; }
        ISubSection17010? SubSection17010 { get; set; }
    }

}
