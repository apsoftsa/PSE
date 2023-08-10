using static PSE.Model.Common.Enumerations;

namespace PSE.Model.Output.Interfaces
{

    public interface IOutputModel
    {

        ManipolationTypes SectionId { get; set; }

        string SectionCode { get; set; }

        string SectionName { get; set; }

    }

}
