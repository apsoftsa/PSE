namespace PSE.Model.Output.Interfaces
{

    public interface IBondsMinorOrEqualTo1Year : IBondsMaturingLessThan5Years { }

    public interface ISection13Content
    {

        IList<IBondsMinorOrEqualTo1Year> BondsMaturingMinorOrEqualTo1Year { get; set; }

    }

}
