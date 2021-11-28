using System;
using System.Text;
using System.Threading.Tasks;
using ServiceBusAdmin.CommandHandlers.Models;
using ServiceBusAdmin.Tool.Subscription.Options;

namespace ServiceBusAdmin.Tool
{
    public class PrintMessageCallback
    {
        private readonly MessageBodyFormatEnum _messageBodyFormat;
        private readonly OutputContentEnum _outputContent;
        private readonly Encoding _encoding;
        private readonly SebaConsole _console;

        public PrintMessageCallback(MessageBodyFormatEnum messageBodyFormat, OutputContentEnum outputContent,
            Encoding encoding, SebaConsole console)
        {
            _messageBodyFormat = messageBodyFormat;
            _outputContent = outputContent;
            _encoding = encoding;
            _console = console;
        }

        public Task Callback(IMessage message)
        {
            switch (_outputContent)
            {
                case OutputContentEnum.Metadata:
                    _console.Info(message.Metadata);
                    break;
                case OutputContentEnum.Body:
                    _console.Info(message.GetBodyString(_encoding));
                    break;
                case OutputContentEnum.All:
                    _console.Info(new
                        { message.Metadata, Body = message.DeserializeBody(_encoding, _messageBodyFormat) });
                    break;
                default:
                    throw new ArgumentOutOfRangeException(_outputContent.ToString());
            }

            return Task.CompletedTask;
        }
    }
}