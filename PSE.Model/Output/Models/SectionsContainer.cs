using PSE.Model.Output.Interfaces;
using Newtonsoft.Json;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public sealed class SectionsContainer
    {

        public IOutputModel? Section3 { get; set; }

        public SectionsContainer() 
        {
            Section3 = null;
        }

        public SectionsContainer(IList<IOutputModel> sections)
        {
            Section3 = null;
            if(sections != null && sections.Any()) 
            { 
                foreach(IOutputModel _section in sections)
                {
                    switch(_section.SectionCode) 
                    {
                        case OUTPUT_SECTION3_CODE:
                            {
                                Section3 = _section;
                            }
                            break;
                    }
                }
            }
        }

    }

}
