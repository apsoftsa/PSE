namespace PSE.Model.Output.Interfaces
{

    public interface IPossibleCommitment
    {

        string Description1 { get; set; }

        string Description2 { get; set; }

        string OpeningDate { get; set; }

        string ExpirationDate { get; set; }

        decimal? CurrentBalance { get; set; }

        decimal? MarketValueReportingCurrency { get; set; }

        int? AccruedInterestReportingCurrency { get; set; }

    }

    public interface IPossibleCommitmentSubSection
    {

        string Name { get; set; }

        IList<IPossibleCommitment> Content { get; set; }

    }

    public interface ISection150Content
    {

        IPossibleCommitmentSubSection SubSection15000 { get; set; }

    }

}