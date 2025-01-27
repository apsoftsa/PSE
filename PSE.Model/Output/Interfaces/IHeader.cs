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

        string? Logo { get; set; }

        string? CompanyName { get; set; }

        string RequestUUID { get; set; }

        string ReferenceString1 { get; set; }

        string ReferenceString2 { get; set; }

        string ReferenceString3 { get; set; }

        string ReferenceString4 { get; set; }

        string ReferenceString5 { get; set; }

        string ReferenceString6 { get; set; }

        string ReferenceString7 { get; set; }

        string ReferenceString8 { get; set; }

        string ReferenceString9 { get; set; }

        string ReferenceString10 { get; set; }

        //IList<IHeaderDescription>? Description { get; set; }

    }

}
