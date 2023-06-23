using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PSE.Builder
{    

    internal static class Utility
    {

        static readonly DefaultContractResolver DefinedContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        };

        public static string? JsonObjectSerialization<T>(this T toSerialize, bool lowerCase = true)
        {
            string? _jsonData = "";
            try
            {
                if (toSerialize != null)
                {
                    var _settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        DefaultValueHandling = DefaultValueHandling.Ignore,                        
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    };
                    if (lowerCase)
                        _settings.ContractResolver = DefinedContractResolver;
                    _jsonData = JsonConvert.SerializeObject(toSerialize, Formatting.Indented, _settings);
                }
            }
            catch
            {
                _jsonData = null;
            }
            return _jsonData;
        }

    }
    
}
