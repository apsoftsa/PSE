using PSE.Model.Common;

namespace PSE.WebApi.ApplicationSettings
{

    public static class ConfigurationReader
    {

        private static void ValidateEnvironmentName(string? environmentName)
        {
            if (environmentName == null) throw new ArgumentNullException(nameof(environmentName));
            var validEnvironments = new List<string>
            {
                "Development", "Testing", "Acceptance", "Production"
            };
            if (string.IsNullOrWhiteSpace(environmentName) || !validEnvironments.Contains(environmentName))
                throw new InvalidOperationException("EnvironmentName non valido. Impostare la variabile di Ambiente ASPNETCORE_ENVIRONMENT ad uno dei seguenti valori [Development, Testing, Acceptance, Production]");
        }

        public static IConfiguration ReadConfiguration()
        {
            string? environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            ValidateEnvironmentName(environmentName);
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: true);
            return builder.Build();
        }

        public static AppSettings ReadAppSettings(this IConfiguration configuration)
        {
            var appSettings = new AppSettings
            {
                DecoderStringConnection = configuration.GetSection("DecoderSettings:ConnectionString").Get<string>(),
                DecoderEnabled = configuration.GetSection("DecoderSettings:Enabled").Get<bool>(),
                DictionariesPath = configuration.GetSection("DictionarySettings:Path").Get<string>(),
                FamUrl = configuration.GetSection("Fam:Url").Get<string>(),
            };
            return appSettings;
        }

    }

}
