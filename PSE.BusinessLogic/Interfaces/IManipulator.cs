using PSE.Dictionary;
using PSE.FamConnector.Multiline;
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

        decimal? TotalAssets { get; }  

        List<PositionClassifications> PositionClassificationsSource { get; }
        ManipolationTypes SectionDestination { get; }

        IOutputModel Manipulate(IPSEDictionaryService dictionaryService, IList<IInputRecord> extractedData, decimal? totalAssets = null);

        string GetObjectNameDestination(IInputRecord inputRecord);

    }

    public interface IMultilineManipulator : IManipulator {

        IOutputModel? Manipulate(IPSEDictionaryService dictionaryService, IMultilineReader multilineReader, IList<IInputRecord> extractedData, decimal? totalAssets = null);

    }

}
