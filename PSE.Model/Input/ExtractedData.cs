using PSE.Model.Input.Log;
using PSE.Model.Input.Interfaces;

namespace PSE.Model.Input
{

    public class ExtractedData : IExtractedData
    {

        public IStreamExtractionLog ExtractionLog { get; set; }
        public IList<IInputRecord> ExtractedItems { get; set; }

        public ExtractedData() 
        { 
            ExtractionLog = new StreamExtractionLog();
            ExtractedItems = new List<IInputRecord>();
        }

        public ExtractedData(IExtractedData source)
        {
            ExtractionLog = source.ExtractionLog != null ? new StreamExtractionLog(source.ExtractionLog) : new StreamExtractionLog();
            ExtractedItems = new List<IInputRecord>();
            if(source.ExtractedItems != null && source.ExtractedItems.Any()) 
            { 
                foreach(var _item in source.ExtractedItems)
                { 
                    ExtractedItems.Add(_item); 
                }
            }
        }

    }

}
