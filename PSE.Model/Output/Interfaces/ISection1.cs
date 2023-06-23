namespace PSE.Model.Output.Interfaces
{

    public interface IAssetStatement
    {

        string Customer { get; set; }

        string Portfolio { get; set; }

        string Date { get; set; }

        string Advisor { get; set; }

    }

    public interface ISection1Content
    {

        IList<IAssetStatement> AssetStatements { get; set; }

    }

}
