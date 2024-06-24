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
                    JObject o = (JObject)token;
                    var propertiesWithDynamicNameAttribute = value.GetType().GetProperties().Where(
                        prop => prop.CustomAttributes.Any(attr => attr.AttributeType == typeof(JsonDynamicNameAttribute))
                    );
                    foreach (var property in propertiesWithDynamicNameAttribute)
                    {
                        CustomAttributeData? dynamicAttributeData = property.CustomAttributes.FirstOrDefault(attr => attr.AttributeType == typeof(JsonDynamicNameAttribute));
                        // Determine what we should rename the property from and to.
                        var currentName = property.Name;
                        if (dynamicAttributeData != null)
                        {
                            object? propertyNameContainingNewName = dynamicAttributeData.ConstructorArguments[0].Value;
                            if (propertyNameContainingNewName != null)
                            {
                                if (value.GetType().GetProperty((string)propertyNameContainingNewName) != null)
                                {
                                    PropertyInfo? propInfo = value.GetType().GetProperty((string)propertyNameContainingNewName);
                                    if (propInfo != null)
                                    {
                                        object? newName = propInfo.GetValue(value);
                                        if (newName != null)
                                        {
                                            // Perform the renaming in the JSON object.
                                            var currentJsonPropertyValue = o[currentName];
                                            var newJsonProperty = new JProperty((string)newName, currentJsonPropertyValue);
                                            if (currentJsonPropertyValue != null && currentJsonPropertyValue.Parent != null)
                                                currentJsonPropertyValue.Parent.Replace(newJsonProperty);
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
            string? jsonData = "";
            try
            {
                if (toSerialize != null)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        DefaultValueHandling = DefaultValueHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        Converters = new JsonConverter[] { new DynamicNameConverter() }
                    };
                    if (lowerCase)
                        settings.ContractResolver = DefinedContractResolver;
                    jsonData = JsonConvert.SerializeObject(toSerialize, Formatting.Indented, settings);
                }
            }
            catch
            {
                jsonData = null;
            }
            return jsonData;
        }

    }

}
