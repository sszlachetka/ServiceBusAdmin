using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using ServiceBusAdmin.CommandHandlers;
using ServiceBusAdmin.CommandHandlers.Models;
using ServiceBusAdmin.CommandHandlers.SendBatch;
using ServiceBusAdmin.Tool.Serialization;

namespace ServiceBusAdmin.Tool.SendBatch
{
    internal class SendMessageParser
    {
        private readonly Encoding _messageBodyEncoding;

        public SendMessageParser(Encoding messageBodyEncoding)
        {
            _messageBodyEncoding = messageBodyEncoding;
        }

        public SendMessageModel Parse(string messageJson)
        {
            var message = Deserialize(messageJson);
            if (message == null) throw new ApplicationException("Message was deserialized to null.");

            var metadata = new SendMessageMetadataModel(
                message.Metadata?.MessageId,
                new Dictionary<string, object>(ParseApplicationProperties(message.Metadata?.ApplicationProperties)));

            return new SendMessageModel(
                ParseBody(message.Body, out var bodyFormat),
                bodyFormat,
                metadata);
        }

        private static RawInputMessage? Deserialize(string messageJson)
        {
            try
            {
                return SebaSerializer.Deserialize<RawInputMessage>(messageJson);
            }
            catch (JsonException e)
            {
                throw new ApplicationException($"Error deserializing message. Details: {e.Message}", e);
            }
        }

        private BinaryData ParseBody(JsonElement body, out MessageBodyFormatEnum bodyFormat)
        {
            var bodyString = GetBodyString(body, out bodyFormat) ?? throw new ApplicationException("Message body is null.");
            
            return new BinaryData(_messageBodyEncoding.GetBytes(bodyString));
        }

        private static string? GetBodyString(JsonElement body, out MessageBodyFormatEnum bodyFormat)
        {
            switch (body.ValueKind)
            {
                case JsonValueKind.Undefined:
                    throw new ApplicationException("Message body is undefined.");
                case JsonValueKind.Null:
                    throw new ApplicationException("Message body is null.");
                case JsonValueKind.String:
                    bodyFormat = MessageBodyFormatEnum.Text;
                    return body.GetString();
                case JsonValueKind.Object:
                    bodyFormat = MessageBodyFormatEnum.Json;
                    return SebaSerializer.Serialize(body);
                default:
                    throw new ApplicationException(
                        $"Message body must be either string or object. Provided value kind {body.ValueKind} is not supported.");
            }
        }

        private static IEnumerable<KeyValuePair<string, object>> ParseApplicationProperties(
            IReadOnlyDictionary<string, JsonElement>? applicationProperties)
        {
            if (applicationProperties == null) yield break;

            foreach (var (key, value) in applicationProperties)
            {
                yield return new KeyValuePair<string, object>(key,
                    ParseApplicationPropertyValue(key, value));
            }
        }

        private static object ParseApplicationPropertyValue(string propertyKey, JsonElement value)
        {
            try
            {
                return value.ValueKind switch
                {
                    JsonValueKind.String => value.GetString() ?? throw new ApplicationException("Null string value."),
                    JsonValueKind.False => false,
                    JsonValueKind.True => true,
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
                    $"Failed to parse '{propertyKey}' application property. Details: {e.Message}",
                    e);
            }
        }
    }
}