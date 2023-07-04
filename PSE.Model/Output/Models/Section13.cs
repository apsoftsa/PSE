using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class BondsMinorOrEqualTo1Year : BondsBase, IBondsMinorOrEqualTo1Year
    {

        public string AmountNominal { get; set; }

        public BondsMinorOrEqualTo1Year() : base()
        { 
            AmountNominal = string.Empty;
        }

        public BondsMinorOrEqualTo1Year(IBondsMinorOrEqualTo1Year source) : base(source)
        {
            AmountNominal = source.AmountNominal;
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section13Content : ISection13Content
    {

        [JsonProperty(propertyName: "bondsMaturing≤1years")]
        public IList<IBondsMinorOrEqualTo1Year> BondsMaturingMinorOrEqualTo1Year { get; set; }

        public Section13Content()
        {
            BondsMaturingMinorOrEqualTo1Year = new List<IBondsMinorOrEqualTo1Year>();
        }

        public Section13Content(ISection13Content source)
        {
            BondsMaturingMinorOrEqualTo1Year = new List<IBondsMinorOrEqualTo1Year>();
            if (source != null)
            {
                if (source.BondsMaturingMinorOrEqualTo1Year != null && source.BondsMaturingMinorOrEqualTo1Year.Any())
                {
                    foreach (IBondsMinorOrEqualTo1Year _bmmoet1y in source.BondsMaturingMinorOrEqualTo1Year)
                    {
                        BondsMaturingMinorOrEqualTo1Year.Add(new BondsMinorOrEqualTo1Year(_bmmoet1y));
                    }
                }
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section13 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection13Content Content { get; set; }

        public Section13() : base(OUTPUT_SECTION13_CODE)
        {
            Content = new Section13Content();
        }

        public Section13(Section13 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section13Content(source.Content);
            else
                Content = new Section13Content();
        }

    }

}
