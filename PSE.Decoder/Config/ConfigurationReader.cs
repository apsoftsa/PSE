using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace PSE.Decoder.Config
{

    internal static class ConfigurationReader
    {

        private static string GetBasePath()
        {
            ProcessModule? _processModule = Process.GetCurrentProcess()?.MainModule;
            return Path.GetDirectoryName(_processModule.FileName);
        }

        public static IConfiguration ReadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(GetBasePath())
                .AddJsonFile("PSE.Decoder.settings.json", optional: false, reloadOnChange: true);
            return builder.Build();
        }

    }

}
