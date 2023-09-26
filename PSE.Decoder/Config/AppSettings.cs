using Microsoft.Extensions.Configuration;

namespace PSE.Decoder.Config
{

    internal class AppSettings
    {

        public string? ConnectionString { get; set; }
        public bool DecoderEnable { get; set; }

        public AppSettings(IConfiguration configuration) 
        {
            ConnectionString = string.Empty;
            if(string.IsNullOrEmpty(configuration["ConnectionString"]) == false)
                ConnectionString = configuration["ConnectionString"];
            DecoderEnable = false;
            if (bool.TryParse(configuration["DecoderEnable"], out bool _enable))
                DecoderEnable = _enable;
        }

    }

}
