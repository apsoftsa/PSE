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

                _logger.LogInformation($"Request multilines data for customers: '{string.Join(',',request.NumIdeList)}'");
                PseReportResult? response = await _famApiClient.SendMultilineRequestAsync(request);
                if (response != null && response.ReportList != null) {
                    if (response.ReportList.Count > 0)
                        _logger.LogInformation($"Multilines found: {response.ReportList.Count}");
                    else
                        _logger.LogInformation("None multiline found!");
                } else
                    _logger.LogWarning("Impossible to detect any multiline.");
                return response;
            } catch (Exception ex) {
                _logger.LogError($"The following error is occurred during the GetCustomersMultiline execution: '{ex.Message}'");
                return null;
            }
        }

    }

}
