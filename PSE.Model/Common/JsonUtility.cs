using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace PSE.Model.Common
{

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class JsonDynamicNameAttribute : Attribute
    {

        public string ObjectPropertyName { get; }

        public JsonDynamicNameAttribute(string objectPropertyName)
        {
            ObjectPropertyName = objectPropertyName;
        }

    }

    public class DynamicNameConverter : JsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            // Only use this converter for classes that contain properties with an JsonDynamicNameAttribute.
            return objectType.IsClass && objectType.GetProperties().Any(prop => prop.CustomAttributes.Any(attr => attr.AttributeType == typeof(JsonDynamicNameAttribute)));
        }

        public override bool CanRead => false;
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            // We do not support deserialization.
            throw new NotImplementedException();
        }

        public override bool CanWrite => true;
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value != null)
            {
                var token = JToken.FromObject(value);
                if (token == null || token.Type != JTokenType.Object)
                {
                    // We should never reach this point because CanConvert() only allows objects with JsonPropertyDynamicNameAttribute to pass through.
                    throw new Exception("Token to be serialized was unexpectedly not an object.");
                }
                else
                {
                    JObject _o = (JObject)token;
                    var _propertiesWithDynamicNameAttribute = value.GetType().GetProperties().Where(
                        _prop => _prop.CustomAttributes.Any(_attr => _attr.AttributeType == typeof(JsonDynamicNameAttribute))
                    );
                    foreach (var _property in _propertiesWithDynamicNameAttribute)
                    {
                        CustomAttributeData? _dynamicAttributeData = _property.CustomAttributes.FirstOrDefault(_attr => _attr.AttributeType == typeof(JsonDynamicNameAttribute));
                        // Determine what we should rename the property from and to.
                        var _currentName = _property.Name;
                        if (_dynamicAttributeData != null)
                        {
                            object? _propertyNameContainingNewName = _dynamicAttributeData.ConstructorArguments[0].Value;
                            if (_propertyNameContainingNewName != null)
                            {
                                if (value.GetType().GetProperty((string)_propertyNameContainingNewName) != null)
                                {
                                    PropertyInfo? _propInfo = value.GetType().GetProperty((string)_propertyNameContainingNewName);
                                    if (_propInfo != null)
                                    {
                                        object? _newName = _propInfo.GetValue(value);
                                        if (_newName != null)
                                        {
                                            // Perform the renaming in the JSON object.
                                            var _currentJsonPropertyValue = _o[_currentName];
                                            var _newJsonProperty = new JProperty((string)_newName, _currentJsonPropertyValue);
                                            if (_currentJsonPropertyValue != null && _currentJsonPropertyValue.Parent != null)
                                                _currentJsonPropertyValue.Parent.Replace(_newJsonProperty);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    token.WriteTo(writer);
                }
            }
        }

    }

    public static class JsonUtility
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
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        Converters = new JsonConverter[] { new DynamicNameConverter() }
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
