namespace PSE.Model.Output.Interfaces
{

    public interface IBondsWithMaturityGreatherThanFiveYears : IObligationsBase { }

    public interface ISection15Content
    {

        IList<IBondsWithMaturityGreatherThanFiveYears> BondsWithMatGreatThanFiveYears { get; set; }

    }

}
