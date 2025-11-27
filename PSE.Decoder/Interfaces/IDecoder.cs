using PSE.Model.Events;

namespace PSE.Decoder
{

    public delegate void ExternalCodifyErrorOccurredEventHandler(object sender, ExternalCodifyRequestEventArgs e);
    public delegate void ExternalCodifiesErrorOccurredEventHandler(object sender, ExternalCodifyRequestEventArgs e);

    public interface IDecoder
    {

        event ExternalCodifyErrorOccurredEventHandler? ExternalCodifyErrorOccurred;
        event ExternalCodifiesErrorOccurredEventHandler? ExternalCodifiesErrorOccurred;

        bool Decode(ExternalCodifyRequestEventArgs e);
        bool Decode(ExternalCodifiesRequestEventArgs e);

        void Dispose();

    }

}
