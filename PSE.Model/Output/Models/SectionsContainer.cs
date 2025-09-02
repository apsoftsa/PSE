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
        public IOutputModel? Section80 { get; set; }
        public IOutputModel? Section90 { get; set; }
        public IOutputModel? Section100 { get; set; }
        public IOutputModel? Section110 { get; set; }
        public IOutputModel? Section130 { get; set; }
        public IOutputModel? Section150 { get; set; }
        public IOutputModel? Section160 { get; set; }
        public IOutputModel? Section170 { get; set; }        
        public IOutputModel? Section190 { get; set; }
        public IOutputModel? Section200 { get; set; }
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
            Section80 = null;
            Section90 = null;
            Section100 = null;
            Section110 = null;
            Section150 = null;
            Section160 = null;
            Section130 = null;
            Section170 = null;
            Section190 = null;
            Section200 = null;
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
                        case Enumerations.ManipolationTypes.AsSection080:
                            Section80 = section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection090:
                            Section90 = section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection110:
                            Section110 = section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection100:
                            Section100 = section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection150:
                            Section150 = section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection160:
                            Section160 = section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection170:
                            Section170 = section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection130:
                            Section130 = section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection190:
                            Section190 = section;
                            break;
                        case Enumerations.ManipolationTypes.AsSection200:
                            Section200 = section;
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
