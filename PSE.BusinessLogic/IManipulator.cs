using PSE.Model.Input.Interfaces;
using PSE.Model.Output.Interfaces;

namespace PSE.BusinessLogic
{

    public interface IManipulator
    {

        IOutputModel Manipulate(IList<IInputRecord> extractedData);

    }

}
