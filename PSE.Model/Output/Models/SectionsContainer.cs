using Newtonsoft.Json;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Constants;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public sealed class SectionsContainer
    {

        public IList<IOutputModel>? Header { get; set; }
        public IOutputModel? Section1 { get; set; }
        public IOutputModel? Section3 { get; set; }
        public IOutputModel? Section8 { get; set; }
        public IOutputModel? Section12 { get; set; }
        public IOutputModel? Section15 { get; set; }

        private void Init()
        {
            Header = null;
            Section1 = null;
            Section3 = null;
            Section8 = null;
            Section12 = null;
            Section15 = null;
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
                        case OUTPUT_SECTION8_CODE:
                            Section8 = _section;
                            break;
                        case OUTPUT_SECTION12_CODE:
                            Section12 = _section;
                            break;
                        case OUTPUT_SECTION15_CODE:
                            Section15 = _section;
                            break;
                    }
                }
            }
        }

    }

}
