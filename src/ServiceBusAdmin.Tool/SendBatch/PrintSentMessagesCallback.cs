using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ServiceBusAdmin.CommandHandlers.Models;
using ServiceBusAdmin.Tool.Serialization;

namespace ServiceBusAdmin.Tool.SendBatch
{
    internal class PrintSentMessagesCallback
    {
        private readonly SebaConsole _console;
        private readonly Encoding _encoding;
        private int _line;

        public PrintSentMessagesCallback(SebaConsole console, Encoding encoding)
        {
            _console = console;
            _encoding = encoding;
        }

        public Task Callback(IReadOnlyCollection<SendMessageModel> messages)
        {
            _console.Verbose($"Messages from lines between {_line + 1} and {_line + messages.Count} ({messages.Count}) were sent successfully.");
            _line += messages.Count;

            foreach (var message in messages)
            {
                _console.Info(
                    new
                    {
                        Body = ToMessageBody(message),
                        message.Metadata
                    });
            }

            return Task.CompletedTask;
        }

        private object ToMessageBody(SendMessageModel message)
        {
            return message.Body.ToMessageBody(_encoding, message.BodyFormat);
        }
    }
}