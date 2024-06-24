using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class BondsMaturingLessThan5Years : BondsBase, IBondsMaturingLessThan5Years
    {

        public BondsMaturingLessThan5Years() : base() { }

        public BondsMaturingLessThan5Years(IBondsMaturingLessThan5Years source) : base(source) { }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section12Content : ISection12Content
    {

        [JsonProperty(propertyName: "bondsExpiration_less5years")]
        public IList<IBondsMaturingLessThan5Years> BondsMaturingLessThan5Years { get; set; }

        public Section12Content()
        {
            BondsMaturingLessThan5Years = new List<IBondsMaturingLessThan5Years>();
        }

        public Section12Content(ISection12Content source)
        {
            BondsMaturingLessThan5Years = new List<IBondsMaturingLessThan5Years>();
            if (source != null)
            {
                if (source.BondsMaturingLessThan5Years != null && source.BondsMaturingLessThan5Years.Any())
                {
                    foreach (IBondsMaturingLessThan5Years bmlt5y in source.BondsMaturingLessThan5Years)
                    {
                        BondsMaturingLessThan5Years.Add(new BondsMaturingLessThan5Years(bmlt5y));
                    }
                }
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section12 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection12Content Content { get; set; }

        public Section12() : base()
        {
            Content = new Section12Content();
        }

        public Section12(Section12 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section12Content(source.Content);
            else
                Content = new Section12Content();
        }

    }

}
