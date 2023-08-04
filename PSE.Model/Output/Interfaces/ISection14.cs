namespace PSE.Model.Output.Interfaces
{

    public interface IObligationsWithMaturityGreatherThanFiveYears : IBondsBase { }

    public interface ISection14Content
    {

        IList<IObligationsWithMaturityGreatherThanFiveYears> ObligationsWithMaturityGreatherThanFiveYears { get; set; }

    }

}
