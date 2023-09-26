using PSE.Model.Events;

namespace PSE.Decoder
{

    public delegate void ExternalCodifyErrorOccurredEventHandler(object sender, ExternalCodifyRequestEventArgs e);

    public interface IDecoder
    {

        event ExternalCodifyErrorOccurredEventHandler? ExternalCodifyErrorOccurred;

        bool Decode(ExternalCodifyRequestEventArgs e);

        void Dispose();

    }

}
