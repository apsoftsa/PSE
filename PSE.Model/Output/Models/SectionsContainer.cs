using Newtonsoft.Json;
using PSE.Model.Common;
using PSE.Model.Output.Interfaces;

namespace PSE.Model.Output.Models
{

    [Serializable]
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public sealed class SectionsContainer
    {

        public IList<IOutputModel>? Header { get; set; }
        public IOutputModel? Section1 { get; set; }
        public IOutputModel? Section3 { get; set; }
        public IOutputModel? Section4 { get; set; }
        public IOutputModel? Section8 { get; set; }
        public IOutputModel? Section9 { get; set; }
        public IOutputModel? Section10 { get; set; }
        public IOutputModel? Section11 { get; set; }
        public IOutputModel? Section12 { get; set; }
        public IOutputModel? Section13 { get; set; }
        public IOutputModel? Section14 { get; set; }
        public IOutputModel? Section15 { get; set; }
        [JsonProperty(propertyName: "section16_17")]
        public IOutputModel? Section16And17 { get; set; }
        [JsonProperty(propertyName: "section18_19")]
        public IOutputModel? Section18And19 { get; set; }
        public IOutputModel? Section20 { get; set; }
        public IList<IOutputModel>? Footer { get; set; }

        private void Init()
        {
            Header = null;
            Section1 = null;
            Section3 = null;
            Section4 = null;    
            Section8 = null;
            Section9 = null;
            Section10 = null;
            Section12 = null;
            Section13 = null;
            Section14 = null;
            Section15 = null;
            Section16And17 = null;
            Section18And19 = null;
            Section20 = null;   
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
                    switch(_section.SectionId) 
                    {
                        case Enumerations.ManipolationTypes.AsHeader:
                            Header = new List<IOutputModel>() { _section };
                            break;
                        case Enumerations.ManipolationTypes.AsSection1:
                            Section1 = _section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection3:
                            Section3 = _section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection4:
                            Section4 = _section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection8:
                            Section8 = _section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection9:
                            Section9 = _section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection10:
                            Section10 = _section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection11:
                            Section11 = _section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection12:
                            Section12 = _section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection13:
                            Section13 = _section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection14:
                            Section14 = _section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection15:
                            Section15 = _section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection16And17:
                            Section16And17 = _section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection18And19:
                            Section18And19 = _section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection20:
                            Section20 = _section;
                            break;
                        case Enumerations.ManipolationTypes.AsFooter:
                            Footer = new List<IOutputModel>() { _section };
                            break;
                    }
                }
            }
        }

    }

}
