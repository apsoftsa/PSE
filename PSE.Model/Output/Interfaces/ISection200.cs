namespace PSE.Model.Output.Interfaces
{
    public interface IEndExtractCustomer
    {
        string Customer { get; set; }
        string CustomerID { get; set; }
        string Portfolio { get; set; }
        int? RiskProfile { get; set; }
        string EsgProfile { get; set; }
    }

    public interface IEndExtractInvestment
    {
        string AssetClass { get; set; }
        string Class { get; set; }  
        decimal? MarketValueReportingCurrency { get; set; }
        decimal? PercentInvestment { get; set; }
    }

    public interface IEndExtractInvestmentChart
    {
        string AssetClass { get; set; }
        decimal? PercentInvestment { get; set; }
    }

    public interface ISubSection20000
    {
        string Name { get; set; }
        IList<IEndExtractCustomer> Content { get; set; }
    }

    public interface ISubSection20010
    {
        string Name { get; set; }
        bool HasMeaningfulData { get; set; }
        IList<IEndExtractInvestment> Content { get; set; }
    }

    public interface ISubSection20020
    {
        string Name { get; set; }
        IList<IEndExtractInvestmentChart> Content { get; set; }
    }

    public interface ISection200Content
    {
        ISubSection20000? SubSection20000 { get; set; }
        ISubSection20010? SubSection20010 { get; set; }
        ISubSection20020? SubSection20020 { get; set; }
    }

}

