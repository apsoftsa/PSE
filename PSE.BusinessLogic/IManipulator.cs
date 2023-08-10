using PSE.Model.Input.Interfaces;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Enumerations;

namespace PSE.BusinessLogic
{

    public interface IManipulator
    {

        PositionClassifications PostionClassificationSource { get; }
        ManipolationTypes SectionDestination { get; }

        IOutputModel Manipulate(IList<IInputRecord> extractedData);

    }

}
