using System.Text.Json;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace PSE.Dictionary {

    public class PSEDictionaryService : IPSEDictionaryService {


        private const string PREFIX_DICTIONARY_FILE_NAME = "PSEDictionary-";        
        private const string DEFAULT_DICTIONARIES_SUBFOLDER = "Dictionaries";
        private const string TRANSLATION_NOT_FOUND = "(Translation Not Found)";

        private static readonly JsonSerializerOptions JsonOpts = new() {
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        };

        private readonly ILogger<PSEDictionaryService> _logger;
        private readonly string _basePath;

        public string DefaultCulture => "en-GB";

        private readonly ConcurrentDictionary<string, IReadOnlyDictionary<string, string>> _cache = new(StringComparer.OrdinalIgnoreCase);
        private readonly ConcurrentDictionary<string, SemaphoreSlim> _loadLocks = new(StringComparer.OrdinalIgnoreCase);

        private static string ResolveBasePath(string basePath) { 
            string path = string.IsNullOrWhiteSpace(basePath) ? DEFAULT_DICTIONARIES_SUBFOLDER : basePath;
            if (!path.EndsWith(Path.DirectorySeparatorChar))
                path += Path.DirectorySeparatorChar;
            return path;
        }

        private string BuildFilePath(string cultureCode) {
            var fileName = $"{PREFIX_DICTIONARY_FILE_NAME}{cultureCode}.json";
            return Path.IsPathRooted(_basePath)
                ? Path.Combine(_basePath, fileName)
                : Path.Combine(AppContext.BaseDirectory, _basePath, fileName);
        }

        private async Task<IReadOnlyDictionary<string, string>> LoadFromDiskOrFallbackAsync(string cultureCode, CancellationToken ct) {
            var path = BuildFilePath(cultureCode);
            if (!File.Exists(path)) {
                _logger.LogWarning("Dictionary {Culture} not found: {Path}. Fallback to {Default}.", cultureCode, path, DefaultCulture);
                path = BuildFilePath(DefaultCulture);
            }
            try {
                await using var fs = File.OpenRead(path);
                var data = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(fs, JsonOpts, ct)
                           ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                return new Dictionary<string, string>(data, StringComparer.OrdinalIgnoreCase);
            } catch (Exception ex) {
                _logger.LogError(ex, "Error occurred reading dictionary {Culture} from {Path}. Used empty dictionary.", cultureCode, path);
                return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }
        }

        private IReadOnlyDictionary<string, string> LoadFromDiskOrFallback(string cultureCode) {
            var path = BuildFilePath(cultureCode);
            if (!File.Exists(path)) {
                _logger.LogWarning("Dictionary {Culture} not found: {Path}. Fallback to {Default}.", cultureCode, path, DefaultCulture);
                path = BuildFilePath(DefaultCulture);
            }
            try {
                using var fs = File.OpenRead(path);
                var data = JsonSerializer.Deserialize<Dictionary<string, string>>(fs, JsonOpts)
                           ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                return new Dictionary<string, string>(data, StringComparer.OrdinalIgnoreCase);
            } catch (Exception ex) {
                _logger.LogError(ex, "Error occurred reading dictionary {Culture} from {Path}. Used empty dictionary.", cultureCode, path);
                return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }
        }

        private async Task<IReadOnlyDictionary<string, string>> GetOrLoadDictionaryAsync(string cultureCode, CancellationToken ct) {
            if (!_cache.TryGetValue(cultureCode, out var dict)) {
                await LoadCultureAsync(cultureCode, ct);
                _cache.TryGetValue(cultureCode, out dict);
            }
            return dict ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        private IReadOnlyDictionary<string, string> GetOrLoadDictionary(string cultureCode) {
            if (!_cache.TryGetValue(cultureCode, out var dict)) {
                LoadCulture(cultureCode);
                _cache.TryGetValue(cultureCode, out dict);
            }
            return dict ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public PSEDictionaryService(string basePath, ILogger<PSEDictionaryService> logger) {
            _logger = logger;
            _basePath = ResolveBasePath(basePath);
            _ = LoadCultureAsync(DefaultCulture); // load in any case the default culture dictionary
        }

        public async Task LoadCultureAsync(string cultureCode, CancellationToken ct = default) {
            if (string.IsNullOrWhiteSpace(cultureCode))
                cultureCode = DefaultCulture;
            if (!_cache.ContainsKey(cultureCode)) {
                var slim = _loadLocks.GetOrAdd(cultureCode, _ => new SemaphoreSlim(1, 1));
                await slim.WaitAsync(ct);
                try {
                    if (!_cache.ContainsKey(cultureCode)) {
                        var dict = await LoadFromDiskOrFallbackAsync(cultureCode, ct);
                        _cache[cultureCode] = dict;
                    }
                } finally {
                    slim.Release();
                    _loadLocks.TryRemove(cultureCode, out _);
                }
            }
        }

        public void LoadCulture(string cultureCode) {
            if (string.IsNullOrWhiteSpace(cultureCode))
                cultureCode = DefaultCulture;
            if (!_cache.ContainsKey(cultureCode)) {
                var slim = _loadLocks.GetOrAdd(cultureCode, _ => new SemaphoreSlim(1, 1));
                slim.Wait();
                try {
                    if (!_cache.ContainsKey(cultureCode)) {
                        var dict = LoadFromDiskOrFallback(cultureCode);
                        _cache[cultureCode] = dict;
                    }
                } finally {
                    slim.Release();
                    _loadLocks.TryRemove(cultureCode, out _);
                }
            }
        }

        public async Task<string> GetTranslationAsync(string key, string cultureCode, CancellationToken ct = default) {
            if (string.IsNullOrWhiteSpace(key))
                return TRANSLATION_NOT_FOUND;
            else {
                var dict = await GetOrLoadDictionaryAsync(cultureCode, ct);
                if (dict.TryGetValue(key, out var value) && !string.IsNullOrEmpty(value))
                    return value;
                else {
                    var defaultDict = await GetOrLoadDictionaryAsync(DefaultCulture, ct);
                    return defaultDict.TryGetValue(key, out var defValue) && !string.IsNullOrEmpty(defValue)
                        ? defValue
                        : TRANSLATION_NOT_FOUND;
                }
            }
        }

        public string GetTranslation(string key, string cultureCode) {
            if (string.IsNullOrWhiteSpace(key))
                return TRANSLATION_NOT_FOUND;
            else {
                var dict = GetOrLoadDictionary(cultureCode);
                if (dict.TryGetValue(key, out var value) && !string.IsNullOrEmpty(value))
                    return value;
                else {
                    var defaultDict = GetOrLoadDictionary(DefaultCulture);
                    return defaultDict.TryGetValue(key, out var defValue) && !string.IsNullOrEmpty(defValue)
                        ? defValue
                        : TRANSLATION_NOT_FOUND;
                }
            }
        }

        public string GetCultureCodeFromLanguage(string language) {
            return (string.IsNullOrEmpty(language) || string.IsNullOrWhiteSpace(language) ? "E" : language.Trim().ToUpper()) switch {
                "F" => "fr-FR",
                "T" => "de-DE",
                "I" => "it-IT",
                _ => "en-GB",
            };
        }

    }

}
