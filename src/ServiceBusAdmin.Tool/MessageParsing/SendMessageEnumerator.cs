using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ServiceBusAdmin.CommandHandlers.Models;

namespace ServiceBusAdmin.Tool.MessageParsing
{
    internal class SendMessageEnumerator : IAsyncEnumerator<SendMessageModel>
    {
        private readonly StreamReader _streamReader;
        private readonly SendMessageParser _sendMessageParser;
        private SendMessageModel? _current;
        private int _lineNumber;

        public SendMessageEnumerator(StreamReader streamReader, SendMessageParser sendMessageParser)
        {
            _current = null;
            _streamReader = streamReader;
            _sendMessageParser = sendMessageParser;
        }

        public ValueTask DisposeAsync()
        {
            _streamReader.Dispose();

            return ValueTask.CompletedTask;
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            var line = await _streamReader.ReadLineAsync();
            if (line == null) return false;
            _lineNumber++;

            _current = Parse(line);
            
            return true;
        }

        private SendMessageModel Parse(string line)
        {
            try
            {
                return _sendMessageParser.Parse(line);
            }
            catch (ApplicationException e)
            {
                throw new ApplicationException($"Failed to parse message from line {_lineNumber}. Details: {e.Message}",
                    e);
            }
        }

        public SendMessageModel Current => _current ?? throw new InvalidOperationException($"Use {nameof(MoveNextAsync)} to set current line");
    }
}