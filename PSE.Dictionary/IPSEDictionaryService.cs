namespace PSE.Dictionary {

    public interface IPSEDictionaryService {

        string DefaultCulture { get; }

        string GetCultureCodeFromLanguage(string language);  

        Task LoadCultureAsync(string cultureCode, CancellationToken ct = default);
        Task<string> GetTranslationAsync(string key, string cultureCode, CancellationToken ct = default);

        void LoadCulture(string cultureCode);
        string GetTranslation(string key, string cultureCode);

    }

}
