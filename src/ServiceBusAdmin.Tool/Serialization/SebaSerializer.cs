using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ServiceBusAdmin.Tool.Serialization
{
    public static class SebaSerializer
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true,
            IgnoreNullValues = true,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };

        public static T? Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, Options);
        }

        public static string Serialize(object value)
        {
            return JsonSerializer.Serialize(value, Options);
        }
    }
}