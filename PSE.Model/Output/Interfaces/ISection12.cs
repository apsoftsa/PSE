namespace PSE.Model.Output.Interfaces
{

    public interface IBondsMaturingLessThan5Years : IBondsBase { }

    public interface ISection12Content
    {

        IList<IBondsMaturingLessThan5Years> BondsMaturingLessThan5Years { get; set; }

    }

}
