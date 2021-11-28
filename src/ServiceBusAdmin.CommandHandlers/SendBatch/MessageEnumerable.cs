using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceBusAdmin.CommandHandlers.SendBatch
{
    internal class MessageEnumerable : IAsyncEnumerable<SendMessageModel>
    {
        private readonly Stream _messageStream;
        private readonly Encoding _encoding;

        public MessageEnumerable(Stream messageStream, Encoding encoding)
        {
            _messageStream = messageStream;
            _encoding = encoding;
        }

        public IAsyncEnumerator<SendMessageModel> GetAsyncEnumerator(CancellationToken cancellationToken = new())
        {
            return new MessageEnumerator(_messageStream, _encoding);
        }
    }

    internal class MessageEnumerator : IAsyncEnumerator<SendMessageModel>
    {
        private readonly StreamReader _messageReader;
        private readonly Stream _messageStream;
        private int _line;

        public MessageEnumerator(Stream messageStream, Encoding encoding)
        {
            _messageStream = messageStream;
            _messageReader = new StreamReader(_messageStream, encoding);
            Current = default!;
        }

        public ValueTask DisposeAsync()
        {
            _messageReader.Dispose();
            return _messageStream.DisposeAsync();
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            var line = await ReadLine();
            if (line == null) return false;

            try
            {
                Current = Parse(line);
                return true;
            }
            catch (ApplicationException e)
            {
                throw new ApplicationException($"Failed to parse message from line {_line}. Details: {e.Message}", e);
            }
        }

        private static SendMessageModel Parse(string line)
        {
            var message = SebaSerializer.Deserialize<RawInputMessage>(line);
            if (message == null) throw new ApplicationException("Line was deserialized to null.");

            return new SendMessageModel(
                message.Body ?? throw new ApplicationException("Message body is null."),
                message.MessageId,
                new Dictionary<string, object>(ParseApplicationProperties(message)));
        }
        
        private static IEnumerable<KeyValuePair<string, object>> ParseApplicationProperties(RawInputMessage message)
        {
            if (message.ApplicationProperties == null) yield break;

            foreach (var (key, value) in message.ApplicationProperties)
            {
                yield return new KeyValuePair<string, object>(key,
                    ParseApplicationPropertyValue(key, (JsonElement)value));
            }
        }

        private static object ParseApplicationPropertyValue(string propertyKey, JsonElement value)
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
                    $"Failed to parse '{propertyKey}' application property. Details: {e.Message}",
                    e);
            }
        }

        private Task<string?> ReadLine()
        {
            _line++;

            return _messageReader.ReadLineAsync();
        }

        public SendMessageModel Current { get; private set; }
    }
}