using PSE.Model.Input.Interfaces;
using PSE.Model.Output.Interfaces;
using static PSE.Model.Common.Enumerations;

namespace PSE.Builder
{

    public interface IBuilder
    {

        IBuiltData Build(IList<IInputRecord> extractedData, BuildFormats formatToBuild);

    }

}
