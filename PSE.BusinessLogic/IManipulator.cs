using PSE.Model.Input.Interfaces;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Enumerations;

namespace PSE.BusinessLogic
{

    public interface IManipulator
    {

        List<PositionClassifications> PositionClassificationsSource { get; }
        ManipolationTypes SectionDestination { get; }

        IOutputModel Manipulate(IList<IInputRecord> extractedData);

        string GetObjectNameDestination(IInputRecord inputRecord);

    }

}
