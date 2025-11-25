using PSE.Dictionary;
using PSE.FamConnector.Multiline;
using PSE.Model.Input.Interfaces;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Enumerations;

namespace PSE.Builder
{

    public interface IBuilder
    {

        IBuiltData Build(IPSEDictionaryService dictionaryService, IMultilineReader multilineReader, IList<IInputRecord> extractedData, BuildFormats formatToBuild);

    }

}
