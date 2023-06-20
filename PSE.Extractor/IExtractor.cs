using PSE.Model.Input.Interfaces;

namespace PSE.Extractor
{

    public interface IExtractor
    {

        IExtractedData Extract(byte[] input);

    }
}
