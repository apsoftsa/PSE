namespace PSE.Model.Input.Interfaces
{

    public interface IExtractedData
    {

        IStreamExtractionLog ExtractionLog { get; set; }
        IList<IInputRecord> ExtractedItems { get; set; }

    }

}
