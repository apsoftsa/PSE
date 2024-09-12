namespace PSE.Model.Output.Interfaces
{

    public interface IAssetStatement
    {

        string? Customer { get; set; }

        string? CustomerID { get; set; }

        IList<ISettled>? Settled { get; set; }

        string? Portfolio { get; set; }

        string? Advisory { get; set; }

    }

    public interface ISection000Content
    {

        IList<IAssetStatement> AssetStatements { get; set; }

    }

}
