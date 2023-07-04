namespace PSE.Model.Output.Interfaces
{

    public interface IBondsMinorOrEqualTo1Year : IBondsBase 
    {

        string AmountNominal { get; set; }

    }

    public interface ISection13Content
    {

        IList<IBondsMinorOrEqualTo1Year> BondsMaturingMinorOrEqualTo1Year { get; set; }

    }

}
