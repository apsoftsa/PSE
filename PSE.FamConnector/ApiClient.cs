using System.Net.Http.Json;
using PSE.Model.Fam;

namespace PSE.FamConnector {

    public class FamApiClient {

        private readonly HttpClient _httpClient;

        public FamApiClient(HttpClient httpClient) {
            _httpClient = httpClient;
        }

        public async Task<PseReportResult?> SendMultilineRequestAsync(PseReportRequest request, CancellationToken ct = default) {
            using var response = await _httpClient.PostAsJsonAsync("/api/pse/PseReport", request, ct);
            if (!response.IsSuccessStatusCode) {
                var error = await response.Content.ReadAsStringAsync(ct);
                throw new Exception($"Errore API HTTPS: {(int)response.StatusCode} {response.ReasonPhrase}\n{error}");
            }
            return await response.Content.ReadFromJsonAsync<PseReportResult>(cancellationToken: ct);
        }

    }

}
