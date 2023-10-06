using PSE.Model.Events;
using PSE.Model.Input.Interfaces;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Enumerations;

namespace PSE.BusinessLogic.Interfaces
{

    public delegate void ExternalCodifyEventHandler(object sender, ExternalCodifyRequestEventArgs e);

    public interface IManipulator
    {

        event ExternalCodifyEventHandler? ExternalCodifyRequest;

        List<PositionClassifications> PositionClassificationsSource { get; }
        ManipolationTypes SectionDestination { get; }

        IOutputModel Manipulate(IList<IInputRecord> extractedData);

        string GetObjectNameDestination(IInputRecord inputRecord);

    }

}
