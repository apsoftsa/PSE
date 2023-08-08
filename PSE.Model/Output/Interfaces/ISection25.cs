namespace PSE.Model.Output.Interfaces
{

    public interface IRelationshipToAdmin
    {

        string? Object { get; set; }

        string? Description { get; set; }

        string? AddressBook { get; set; }

        string? Currency { get; set; }

        decimal? CurrentBalance { get; set; }

        decimal? MarketValueReportingCurrency { get; set; }

    }

    public interface ISection25Content
    {

        IList<IRelationshipToAdmin> RelationshipNonTransferedToAdmin { get; set; }

        string? TotalAddressBook { get; set; }

        decimal? TotalMarketValueReportingCurrency { get; set; }

        IList<IRelationshipToAdmin> RelationshipTransferedToAdmin { get; set; }

    }

}