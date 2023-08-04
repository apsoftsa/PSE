namespace PSE.Model.Output.Interfaces
{

    public interface IFundDetails : IFundBase { }

    public interface ISection16And17Content
    {

        IList<IFundDetails> BondFunds { get; set; }

        IList<IFundDetails> EquityFunds { get; set; }

        IList<IFundDetails> MixedFunds { get; set; }

        IList<IFundDetails> RealEstateFunds { get; set; }

        IList<IFundDetails> MetalFunds { get; set; }

    }

}