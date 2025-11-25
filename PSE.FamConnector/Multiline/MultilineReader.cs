using Microsoft.Extensions.Logging;
using PSE.Model.Fam;

namespace PSE.FamConnector.Multiline {

    public class MultilineReader : IMultilineReader {

        private readonly ILogger<MultilineReader> _logger;
        private readonly FamApiClient _famApiClient;

        public MultilineReader(string famUrl, ILogger<MultilineReader> logger) {
            _logger = logger;
            _famApiClient = new FamApiClient(new HttpClient {
                BaseAddress = new Uri(famUrl)
            });
        }

        public async Task<PseReportResult?> GetCustomersMultiline(PseReportRequest request) {
            try {
                return await _famApiClient.SendMultilineRequestAsync(request);
            } catch (Exception ex) {
                _logger.LogError($"The following error is occurred during the GetCustomersMultiline execution: '{ex.Message}'");
                return null;
            }
        }

    }

}
