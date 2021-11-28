using System.Text.Json;

namespace ServiceBusAdmin.CommandHandlers
{
    public static class SebaSerializer
    {
        private static readonly JsonSerializerOptions Options = new ()
        {
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true
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