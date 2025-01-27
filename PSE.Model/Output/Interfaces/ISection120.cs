namespace PSE.Model.Output.Interfaces
{

    public interface IFinancialSolutionMortgage
    {
        string Description1 { get; set; }
        string Description2 { get; set; }
        decimal? PercentRate { get; set; }
        string OpeningDate { get; set; }
        string ExpirationDate { get; set; }
        string CurrentBalance { get; set; }
        decimal? MarketValueReportingCurrency { get; set; }
        decimal? AccruedInterestReportingCurrency { get; set; }
        decimal? Exchange { get; set; }
    }

    public interface IFinancialSolutionAccount
    {
        string Description1 { get; set; }
        string Description2 { get; set; }
        decimal? PercentRate { get; set; }
        decimal? AdvanceValue { get; set; }
        decimal? QuantityUsed { get; set; }
        decimal? QuantityAvailable { get; set; }
        string OpeningDate { get; set; }
        decimal? MarketValueReportingCurrency { get; set; }
        decimal? Exchange { get; set; }
    }

    public interface IFinancialSolutionFixed : IFinancialSolutionMortgage
    {

        decimal? PercentWeight { get; set; }
    }   

    public interface ISubSection12000
    {
        string Name { get; set; }
        IList<IFinancialSolutionMortgage> Content { get; set; }
    }

    public interface ISubSection12010
    {
        string Name { get; set; }
        IList<IFinancialSolutionAccount> Content { get; set; }
    }

    public interface ISubSection12020
    {
        string Name { get; set; }
        IList<IFinancialSolutionFixed> Content { get; set; }
    }

    public interface ISection120Content
    {
        ISubSection12000? SubSection12000 { get; set; }
        ISubSection12010? SubSection12010 { get; set; }
        ISubSection12020? SubSection12020 { get; set; }
    }

}
