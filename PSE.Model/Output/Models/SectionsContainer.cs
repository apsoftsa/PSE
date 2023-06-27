using Newtonsoft.Json;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public sealed class SectionsContainer
    {

        [JsonProperty(propertyName: OUTPUT_HEADER_CODE)]
        public IList<IOutputModel>? Header { get; set; }
        [JsonProperty(propertyName: OUTPUT_SECTION1_CODE)]
        public IOutputModel? Section1 { get; set; }
        [JsonProperty(propertyName: OUTPUT_SECTION3_CODE)]
        public IOutputModel? Section3 { get; set; }
        [JsonProperty(propertyName: OUTPUT_SECTION4_CODE)]
        public IOutputModel? Section4 { get; set; }
        [JsonProperty(propertyName: OUTPUT_SECTION8_CODE)]
        public IOutputModel? Section8 { get; set; }
        [JsonProperty(propertyName: OUTPUT_SECTION12_CODE)]
        public IOutputModel? Section12 { get; set; }
        [JsonProperty(propertyName: OUTPUT_SECTION15_CODE)]
        public IOutputModel? Section15 { get; set; }
        [JsonProperty(propertyName: OUTPUT_SECTION18AND19_CODE)]
        public IOutputModel? Section18And19 { get; set; }
        [JsonProperty(propertyName: OUTPUT_FOOTER_CODE)]
        public IList<IOutputModel>? Footer { get; set; }

        private void Init()
        {
            Header = null;
            Section1 = null;
            Section3 = null;
            Section4 = null;    
            Section8 = null;
            Section12 = null;
            Section15 = null;
            Section18And19 = null;
            Footer = null;  
        }

        public SectionsContainer() 
        {
             Init();
        }

        public SectionsContainer(IList<IOutputModel> sections)
        {
            Init();
            if (sections != null && sections.Any()) 
            { 
                foreach(IOutputModel _section in sections)
                {
                    switch(_section.SectionCode) 
                    {
                        case OUTPUT_HEADER_CODE:
                            Header = new List<IOutputModel>() { _section };
                            break;
                        case OUTPUT_SECTION1_CODE:
                            Section1 = _section;
                            break;
                        case OUTPUT_SECTION3_CODE:
                            Section3 = _section;
                            break;
                        case OUTPUT_SECTION4_CODE:
                            Section4 = _section;
                            break;
                        case OUTPUT_SECTION8_CODE:
                            Section8 = _section;
                            break;
                        case OUTPUT_SECTION12_CODE:
                            Section12 = _section;
                            break;
                        case OUTPUT_SECTION15_CODE:
                            Section15 = _section;
                            break;
                        case OUTPUT_SECTION18AND19_CODE:
                            Section18And19 = _section;
                            break;
                        case OUTPUT_FOOTER_CODE:
                            Footer = new List<IOutputModel>() { _section };
                            break;
                    }
                }
            }
        }

    }

}
