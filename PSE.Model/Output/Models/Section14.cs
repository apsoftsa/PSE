using Newtonsoft.Json;
using PSE.Model.Output.Common;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ObligationsWithMaturityGreatherThanFiveYears : BondsBase, IObligationsWithMaturityGreatherThanFiveYears
    {

        public ObligationsWithMaturityGreatherThanFiveYears() : base() { }

        public ObligationsWithMaturityGreatherThanFiveYears(IObligationsWithMaturityGreatherThanFiveYears source) : base(source) { }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section14Content : ISection14Content
    {

        [JsonProperty(propertyName: "bondsExpiration_more5years")]
        public IList<IObligationsWithMaturityGreatherThanFiveYears> ObligationsWithMaturityGreatherThanFiveYears { get; set; }

        public Section14Content()
        {
            ObligationsWithMaturityGreatherThanFiveYears = new List<IObligationsWithMaturityGreatherThanFiveYears>();
        }

        public Section14Content(ISection14Content source)
        {
            ObligationsWithMaturityGreatherThanFiveYears = new List<IObligationsWithMaturityGreatherThanFiveYears>();
            if (source != null)
            {
                if (source.ObligationsWithMaturityGreatherThanFiveYears != null && source.ObligationsWithMaturityGreatherThanFiveYears.Any())
                {
                    foreach (IObligationsWithMaturityGreatherThanFiveYears owmgt5y in source.ObligationsWithMaturityGreatherThanFiveYears)
                    {
                        ObligationsWithMaturityGreatherThanFiveYears.Add(new ObligationsWithMaturityGreatherThanFiveYears(owmgt5y));
                    }
                }
            }
        }

    }

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Section14 : OutputModel
    {

        [JsonProperty(propertyName: "name", Order = 1)]
        public string FakeName { get { return base.SectionName; } private set { } }

        [JsonProperty(Order = 2)]
        public ISection14Content Content { get; set; }

        public Section14() : base()
        {
            Content = new Section14Content();
        }

        public Section14(Section14 source) : base(source)
        {
            if (source.Content != null)
                Content = new Section14Content(source.Content);
            else
                Content = new Section14Content();
        }

    }

}
