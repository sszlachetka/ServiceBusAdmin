using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.CommandHandlers.FilesAccess;
using ServiceBusAdmin.CommandHandlers.SendBatch;
using ServiceBusAdmin.Tool.Arguments;
using ServiceBusAdmin.Tool.Options;
using ServiceBusAdmin.Tool.SendBatch;
using ServiceBusAdmin.Tool.Serialization;

namespace ServiceBusAdmin.Tool.Topic
{
    public class SendBatchCommand : SebaCommand
    {
        private readonly Func<string> _getTopicName;
        private readonly Func<string> _getInputFile;
        private readonly Func<Encoding> _getInputFileEncoding;
        private readonly Func<Encoding> _getSendMessageEncoding;

        public SendBatchCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Name = "send-batch";
            Command.Description = "Send batch of messages to a topic.";
            _getTopicName = Command.ConfigureTopicNameArgument();
            _getInputFile = Command.ConfigureInputFileOption();
            _getInputFileEncoding = Command.ConfigureEncodingNameOption("Input file encoding", "--input-file-encoding");
            _getSendMessageEncoding = Command.ConfigureEncodingNameOption("Send message encoding", "--send-message-encoding");
        }

        protected override async Task Execute(CancellationToken cancellationToken)
        {
            await using var fileStream = await Mediator.Send(new ReadFile(_getInputFile()), cancellationToken);
            var enumerator = new SendMessageEnumerator(new StreamReader(fileStream, _getInputFileEncoding()),
                new SendMessageParser(_getSendMessageEncoding()));
            var print = new PrintSentMessagesCallback(Console, _getSendMessageEncoding());

            var sendBatchMessages = new SendBatchMessages(_getTopicName(), enumerator, print.Callback);

            await Mediator.Send(sendBatchMessages, cancellationToken);
        }
        
        private class PrintSentMessagesCallback
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
}