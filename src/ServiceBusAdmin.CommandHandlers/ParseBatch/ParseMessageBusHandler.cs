using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ServiceBusAdmin.CommandHandlers.Models;

namespace ServiceBusAdmin.CommandHandlers.ParseBatch
{
    public class ParseMessageBusHandler : IRequestHandler<ParseMessageBatch, SendMessageModel[]>
    {
        public Task<SendMessageModel[]> Handle(ParseMessageBatch request, CancellationToken cancellationToken)
        {
            //TODO: Refactor this method
            var serializationOptions = new JsonSerializerOptions()
            {
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                AllowTrailingCommas = true
            };
            var content = request.Content;
            var messages = JsonSerializer.Deserialize<RawInputMessage[]>(content, serializationOptions);
            if (messages == null || messages.Length == 0)
            {
                // TODO: provide link to documentation describing format of the input file
                throw new ApplicationException(
                    "Provided content was deserialized to null or empty array of messages. " +
                    $"Please provide content in valid format. Provided content was {content[..Math.Min(100, content.Length)]}");
            }

            var result = new SendMessageModel[messages.Length];
            for (var i = 0; i < messages.Length; i++)
            {
                var message = messages[i];
                result[i] = new SendMessageModel(
                    message.Body ??
                    throw new ApplicationException(
                        $"Failed to parse message at index {i}. Body is was not deserialized."),
                    message.MessageId,
                    message.BodyFormat,
                    new Dictionary<string, object>(ParseApplicationProperties(i, message)));
            }

            return Task.FromResult(result);
        }

        private static IEnumerable<KeyValuePair<string, object>> ParseApplicationProperties(int messageIndex, RawInputMessage message)
        {
            if (message.ApplicationProperties == null) yield break;

            foreach (var (key, value) in message.ApplicationProperties)
            {
                yield return new KeyValuePair<string, object>(key,
                    ParseApplicationPropertyValue(messageIndex, key, (JsonElement)value));
            }
        }

        private static object ParseApplicationPropertyValue(int messageIndex, string propertyKey, JsonElement value)
        {
            try
            {
                return value.ValueKind switch
                {
                    JsonValueKind.String => value.GetString() ?? throw new ApplicationException("Null string value."),
                    JsonValueKind.Number => value.TryGetInt64(out var parsedInt64)
                        ? parsedInt64
                        : value.TryGetDouble(out var parsedDouble) 
                            ? parsedDouble : throw new ApplicationException("Value cannot be converted to neither Int64 nor Double."),
                    _ => throw new ArgumentOutOfRangeException($"{value.ValueKind} value kind is not supported.")
                };
            }
            catch (Exception e)
            {
                throw new ApplicationException(
                    $"Failed to parse '{propertyKey}' application property for message index {messageIndex}. Details: {e.Message}",
                    e);
            }
        }
    }
}