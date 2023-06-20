using PSE.Model.Input.Interfaces;
using PSE.Model.Output.Interfaces;

namespace PSE.BusinessLogic
{

    public interface IManipolator
    {

        IOutputModel Manipulate(IList<IInputRecord> extractedData);

    }

}
