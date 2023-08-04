using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class BondsWithMaturityGreatherThanFiveYears : ObligationsBase, IBondsWithMaturityGreatherThanFiveYears
    {

        public BondsWithMaturityGreatherThanFiveYears() : base() { }

        public BondsWithMaturityGreatherThanFiveYears(IBondsWithMaturityGreatherThanFiveYears source) : base(source) { }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section15Content : ISection15Content
    {

        [JsonProperty(propertyName: "bondsWithMaturityGreatherThanFiveYears")]
        public IList<IBondsWithMaturityGreatherThanFiveYears> BondsWithMatGreatThanFiveYears { get; set; }

        public Section15Content()
        {
            BondsWithMatGreatThanFiveYears = new List<IBondsWithMaturityGreatherThanFiveYears>();
        }

        public Section15Content(ISection15Content source)
        {
            BondsWithMatGreatThanFiveYears = new List<IBondsWithMaturityGreatherThanFiveYears>();
            if (source != null)
            {
                if (source.BondsWithMatGreatThanFiveYears != null && source.BondsWithMatGreatThanFiveYears.Any())
                {
                    foreach (IBondsWithMaturityGreatherThanFiveYears _bwmgt5y in source.BondsWithMatGreatThanFiveYears)
                    {
                        BondsWithMatGreatThanFiveYears.Add(new BondsWithMaturityGreatherThanFiveYears(_bwmgt5y));
                    }
                }
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section15 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection15Content Content { get; set; }

        public Section15() : base(OUTPUT_SECTION15_CODE)
        {
            Content = new Section15Content();
        }

        public Section15(Section15 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section15Content(source.Content);
            else
                Content = new Section15Content();
        }

    }

}
