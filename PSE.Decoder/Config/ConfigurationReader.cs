using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace PSE.Decoder.Config
{

    internal static class ConfigurationReader
    {

        private static string? GetBasePath()
        {
            ProcessModule? _processModule = Process.GetCurrentProcess()?.MainModule;
            return _processModule != null ? Path.GetDirectoryName(_processModule.FileName) : string.Empty;
        }

        public static IConfiguration? ReadConfiguration()
        {
            string ? _basePath = GetBasePath();
            if (_basePath != null)
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(_basePath)
                    .AddJsonFile("PSE.Decoder.settings.json", optional: false, reloadOnChange: true);
                return builder.Build();
            }
            else
                return null;
        }

    }

}
