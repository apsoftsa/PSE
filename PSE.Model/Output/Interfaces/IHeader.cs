namespace PSE.Model.Output.Interfaces
{

    public interface IHeaderDescription
    {
       
        IList<ISettled> Settled { get; set; }

        string Assessment { get; set; }

        string CustomerID { get; set; }

    }

    public interface IHeaderContent : IOutputModel
    {

        string Logo { get; set; }

        string CompanyName { get; set; }

        string RequestUUID { get; set; }

        IList<IHeaderDescription>? Description { get; set; }

    }

}
