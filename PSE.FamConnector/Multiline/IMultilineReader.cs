using PSE.Model.Fam;

namespace PSE.FamConnector.Multiline {

    public interface IMultilineReader {

        Task<PseReportResult?> GetCustomersMultiline(PseReportRequest request);

    }

}
