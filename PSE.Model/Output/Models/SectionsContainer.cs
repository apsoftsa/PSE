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
        public IOutputModel? Section0 { get; set; }
        public IOutputModel? Section10 { get; set; }
        public IOutputModel? Section20 { get; set; }
        public IOutputModel? Section40 { get; set; }
        public IOutputModel? Section60 { get; set; }
        public IOutputModel? Section70 { get; set; }
        //public IOutputModel? Section9 { get; set; }
        //public IOutputModel? Section10 { get; set; }
        //public IOutputModel? Section11 { get; set; }
        public IOutputModel? Section80 { get; set; }
        //public IOutputModel? Section13 { get; set; }
        //public IOutputModel? Section14 { get; set; }
        public IOutputModel? Section90 { get; set; }
        //[JsonProperty(propertyName: "section16_17")]
        //public IOutputModel? Section16And17 { get; set; }
        [JsonProperty(propertyName: "section18_19")]
        public IOutputModel? Section18And19 { get; set; }
        public IOutputModel? Section20Old { get; set; }
        public IOutputModel? Section21 { get; set; }
        public IOutputModel? Section22 { get; set; }
        public IOutputModel? Section23 { get; set; }
        public IOutputModel? Section24 { get; set; }
        public IOutputModel? Section25 { get; set; }
        public IOutputModel? Section26 { get; set; }
        public IList<IOutputModel>? Footer { get; set; }

        private void Init()
        {
            Header = null;
            Section0 = null;
            Section10 = null;
            Section20 = null;
            Section40 = null;
            Section60 = null;
            Section70 = null;
            //Section9 = null;
            //Section10 = null;
            Section80 = null;
            //Section13 = null;
            //Section14 = null;
            Section90 = null;
            //Section16And17 = null;
            Section18And19 = null;
            Section20Old = null;
            Section21 = null;
            Section22 = null;
            Section23 = null;
            Section24 = null;   
            Section25 = null;   
            Section26 = null;   
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
                foreach(IOutputModel section in sections)
                {
                    switch(section.SectionId) 
                    {
                        case Enumerations.ManipolationTypes.AsHeader:
                            Header = new List<IOutputModel>() { section };
                            break;
                        case Enumerations.ManipolationTypes.AsSection000:
                            Section0 = section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection010:
                            Section10 = section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection020:
                            Section20 = section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection040:
                            Section40 = section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection060:
                            Section60 = section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection070:
                            Section70 = section;
                            break;
                            /*
                        case Enumerations.ManipolationTypes.AsSection9:
                            Section9 = section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection10:
                            Section10Old = section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection11:
                            Section11 = section;
                            break;
                            */
                        case Enumerations.ManipolationTypes.AsSection080:
                            Section80 = section;
                            break;
                        /*
                        case Enumerations.ManipolationTypes.AsSection13:
                            Section13 = section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection14:
                            Section14 = section;
                            break;
                        */
                        case Enumerations.ManipolationTypes.AsSection090:
                            Section90 = section;
                            break;
                        //case Enumerations.ManipolationTypes.AsSection16And17:
                        //    Section16And17 = section;
                        //    break;
                        case Enumerations.ManipolationTypes.AsSection18And19:
                            Section18And19 = section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection100:
                            Section20Old = section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection21:
                            Section21 = section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection22:
                            Section22 = section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection23:
                            Section23 = section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection24:
                            Section24 = section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection25:
                            Section25 = section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection26:
                            Section26 = section;
                            break;
                        case Enumerations.ManipolationTypes.AsFooter:
                            Footer = new List<IOutputModel>() { section };
                            break;
                    }
                }
            }
        }

    }

}
