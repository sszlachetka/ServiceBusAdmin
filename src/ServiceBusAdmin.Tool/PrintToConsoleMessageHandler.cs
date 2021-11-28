using System;
using System.Text;
using System.Threading.Tasks;
using ServiceBusAdmin.CommandHandlers;
using ServiceBusAdmin.CommandHandlers.Models;
using ServiceBusAdmin.Tool.Subscription.Options;

namespace ServiceBusAdmin.Tool
{
    public class PrintToConsoleMessageHandler
    {
        private readonly MessageBodyFormatEnum _messageBodyFormat;
        private readonly OutputContentEnum _outputContent;
        private readonly Encoding _encoding;
        private readonly SebaConsole _console;

        public PrintToConsoleMessageHandler(MessageBodyFormatEnum messageBodyFormat, OutputContentEnum outputContent,
            Encoding encoding, SebaConsole console)
        {
            _messageBodyFormat = messageBodyFormat;
            _outputContent = outputContent;
            _encoding = encoding;
            _console = console;
        }

        public Task Handle(IMessage message)
        {
            switch (_outputContent)
            {
                case OutputContentEnum.Metadata:
                    _console.Info(message.MapToMetadata());
                    break;
                case OutputContentEnum.Body:
                    _console.Info(message.GetBodyString(_encoding));
                    break;
                case OutputContentEnum.All:
                    _console.Info(message.MapToModel(_encoding, _messageBodyFormat));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(_outputContent.ToString());
            }

            return Task.CompletedTask;
        }
    }
}