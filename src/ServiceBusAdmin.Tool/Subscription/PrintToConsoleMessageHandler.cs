using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ServiceBusAdmin.CommandHandlers;
using ServiceBusAdmin.Tool.Subscription.Options;

namespace ServiceBusAdmin.Tool.Subscription
{
    public class PrintToConsoleMessageHandler
    {
        private readonly OutputContentEnum _outputContent;
        private readonly Encoding _encoding;
        private readonly SebaConsole _console;

        public PrintToConsoleMessageHandler(OutputContentEnum outputContent, string encodingName, SebaConsole console)
        {
            _outputContent = outputContent;
            _encoding = Encoding.GetEncoding(encodingName);
            _console = console;
        }

        public Task Handle(IMessage message)
        {
            switch (_outputContent)
            {
                case OutputContentEnum.Metadata:
                    _console.Info(new MessageMetadata(message.SequenceNumber, message.MessageId,
                        message.ApplicationProperties));
                    break;
                case OutputContentEnum.Body:
                    _console.Info(message.Body.ToString());
                    break;
                case OutputContentEnum.All:
                    _console.Info(new Message(message.SequenceNumber, message.MessageId,
                        message.ApplicationProperties, JsonSerializer.Deserialize<dynamic>(message.Body.ToString())));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(_outputContent.ToString());
            }

            return Task.CompletedTask;
        }
    }
}