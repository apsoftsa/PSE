using PSE.Model.Input.Interfaces;
using static PSE.Model.Common.Enumerations;

namespace PSE.Model.Input.Log
{

    public class RecordExtractionLog : IRecordExtractionLog
    {

        public string? RecordInnerContent { get; set; }
        public string? RecordTypeName { get; set; }
        public string? FurtherMessage { get; set; }
        public int? LineNumber { get; set; }
        public Exception? ExceptionOccurred { get; set; }

        public RecordExtractionLog()
        {
            RecordInnerContent = string.Empty;
            RecordTypeName = string.Empty;  
            FurtherMessage = string.Empty;
            LineNumber = -1;
            ExceptionOccurred = null;
        }

        public RecordExtractionLog(IRecordExtractionLog source)
        {
            FurtherMessage = source.RecordInnerContent;
            RecordInnerContent = source.RecordInnerContent;
            RecordTypeName = source.RecordTypeName;
            LineNumber = source.LineNumber;
            ExceptionOccurred = source.ExceptionOccurred;
        }

    }

    public class StreamExtractionLog : IStreamExtractionLog
    {

        public StreamAcquisitionOutcomes Outcome { get; set; }
        public int StreamLength { get; set; }   
        public DateTime? AcquisitionStart { get; set; }
        public DateTime? AcquisitionEnd { get; set; }
        public IList<IRecordExtractionLog> RecordsLog { get; set; }

        public StreamExtractionLog()
        {
            Outcome = StreamAcquisitionOutcomes.Unknown;
            StreamLength = 0;
            AcquisitionStart = null;
            AcquisitionEnd = null;
            RecordsLog = new List<IRecordExtractionLog>();
        }

        public StreamExtractionLog(IStreamExtractionLog source)
        {
            Outcome = source.Outcome;
            StreamLength = source.StreamLength; 
            AcquisitionStart = source.AcquisitionStart;
            AcquisitionEnd = source.AcquisitionEnd;
            RecordsLog = new List<IRecordExtractionLog>();
            if (source.RecordsLog != null && source.RecordsLog.Any())
            {
                foreach (IRecordExtractionLog _record in source.RecordsLog)
                {
                    RecordsLog.Add(new RecordExtractionLog(_record));
                }
            }
        }

    }

}
