using FileHelpers;
using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class FundDetail : ObligationsBase, IFundDetails
    {

        public FundDetail() : base() { }

        public FundDetail(IFundDetails source) : base(source) { }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section16And17Content : ISection16And17Content
    {

        [JsonIgnore]
        public IList<IFundDetails> BondFunds { get; set; }

        [JsonProperty("bondFunds", Order = 1)]
        public IList<IFundDetails>? SerializationBondFund
        {
            get { return BondFunds != null && BondFunds.Any() ? (List<IFundDetails>)BondFunds : null; }
            private set { }
        }

        [JsonIgnore]
        public IList<IFundDetails> EquityFunds { get; set; }

        [JsonProperty("equityFunds", Order = 2)]
        public IList<IFundDetails>? SerializationEquityFund
        {
            get { return EquityFunds != null && EquityFunds.Any() ? (List<IFundDetails>)EquityFunds : null; }
            private set { }
        }

        [JsonIgnore]
        public IList<IFundDetails> MixedFunds { get; set; }

        [JsonProperty("mixedFunds", Order = 3)]
        public IList<IFundDetails>? SerializationMixedFund
        {
            get { return MixedFunds != null && MixedFunds.Any() ? (List<IFundDetails>)MixedFunds : null; }
            private set { }
        }

        [JsonIgnore]
        public IList<IFundDetails> RealEstateFunds { get; set; }
        
        [JsonProperty("realEstateFunds", Order = 4)]
        public IList<IFundDetails>? SerializationRealEstateFund
        {
            get { return RealEstateFunds != null && RealEstateFunds.Any() ? (List<IFundDetails>)RealEstateFunds : null; }
            private set { }
        }

        [JsonIgnore]
        public IList<IFundDetails> MetalFunds { get; set; }

        [JsonProperty("metalFunds", Order = 5)]
        public IList<IFundDetails>? SerializationMetalFund
        {
            get { return MetalFunds != null && MetalFunds.Any() ? (List<IFundDetails>)MetalFunds : null; }
            private set { }
        }

        public Section16And17Content()
        {
            BondFunds = new List<IFundDetails>();
            EquityFunds = new List<IFundDetails>();
            MixedFunds = new List<IFundDetails>();
            RealEstateFunds = new List<IFundDetails>();
            MetalFunds = new List<IFundDetails>();
        }

        public Section16And17Content(ISection16And17Content source)
        {
            BondFunds = new List<IFundDetails>();
            if (source.BondFunds != null && source.BondFunds.Any())
            {
                foreach (IFundDetails _fund in source.BondFunds)
                {
                    BondFunds.Add(new FundDetail(_fund));
                }
            }
            EquityFunds = new List<IFundDetails>();
            if (source.EquityFunds != null && source.EquityFunds.Any())
            {
                foreach (IFundDetails _fund in source.EquityFunds)
                {
                    EquityFunds.Add(new FundDetail(_fund));
                }
            }
            MixedFunds = new List<IFundDetails>();
            if (source.MixedFunds != null && source.MixedFunds.Any())
            {
                foreach (IFundDetails _fund in source.MixedFunds)
                {
                    MixedFunds.Add(new FundDetail(_fund));
                }
            }
            RealEstateFunds = new List<IFundDetails>();
            if (source.RealEstateFunds != null && source.RealEstateFunds.Any())
            {
                foreach (IFundDetails _fund in source.RealEstateFunds)
                {
                    RealEstateFunds.Add(new FundDetail(_fund));
                }
            }
            MetalFunds = new List<IFundDetails>();
            if (source.MetalFunds != null && source.MetalFunds.Any())
            {
                foreach (IFundDetails _fund in source.MetalFunds)
                {
                    MetalFunds.Add(new FundDetail(_fund));
                }
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section16And17 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection16And17Content Content { get; set; }

        public Section16And17() : base()
        {
            Content = new Section16And17Content();
        }

        public Section16And17(Section16And17 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section16And17Content(source.Content);
            else
                Content = new Section16And17Content();
        }

    }

}
