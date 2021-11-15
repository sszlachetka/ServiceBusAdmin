using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ServiceBusAdmin.Client;
using ServiceBusAdmin.Tool.Subscription.Options;

namespace ServiceBusAdmin.Tool.Subscription
{
    public class PrintToConsoleMessageHandler
    {
        private readonly OutputFormatOption _outputFormat;
        private readonly Encoding _encoding;
        private readonly SebaConsole _console;

        public PrintToConsoleMessageHandler(OutputFormatOption outputFormat, string encodingName, SebaConsole console)
        {
            _outputFormat = outputFormat;
            _encoding = Encoding.GetEncoding(encodingName);
            _console = console;
        }

        public Task Handle(IMessage message)
        {
            _console.Info(_outputFormat.Value,
                _outputFormat.IncludesMessageBody ? _encoding.GetString(message.Body) : string.Empty,
                message.SequenceNumber,
                message.MessageId,
                _outputFormat.IncludesApplicationProperties
                    ? JsonSerializer.Serialize(message.ApplicationProperties)
                    : string.Empty);

            return Task.CompletedTask;
        }
    }
}