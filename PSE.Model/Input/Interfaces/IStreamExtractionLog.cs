using static PSE.Model.Common.Enumerations;

namespace PSE.Model.Input.Interfaces
{

    public interface IRecordExtractionLog
    {

        string? RecordInnerContent { get; set; }
        string? RecordTypeName { get; set; }
        string? FurtherMessage { get; set; }
        int? LineNumber { get; set; }
        Exception? ExceptionOccurred { get; set; }

    }

    public interface IStreamExtractionLog
    {

        StreamAcquisitionOutcomes Outcome { get; set; }
        int StreamLength { get; set; }  
        DateTime? AcquisitionStart { get; set; }
        DateTime? AcquisitionEnd { get; set; }
        IList<IRecordExtractionLog> RecordsLog { get; set; }

    }

}
