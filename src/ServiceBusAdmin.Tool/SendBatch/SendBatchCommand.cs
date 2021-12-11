using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.CommandHandlers.FilesAccess;
using ServiceBusAdmin.CommandHandlers.SendBatch;
using ServiceBusAdmin.Tool.Arguments;
using ServiceBusAdmin.Tool.MessageParsing;
using ServiceBusAdmin.Tool.Options;

namespace ServiceBusAdmin.Tool.SendBatch
{
    public class SendBatchCommand: SebaCommand
    {
        private readonly Func<string> _getQueueOrTopicName;
        private readonly Func<string> _getInputFile;
        private readonly Func<Encoding> _getInputFileEncoding;
        private readonly Func<Encoding> _getSendMessageEncoding;

        public SendBatchCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Name = "send-batch";
            Command.Description = "Send batch of messages to given entity.";
            _getQueueOrTopicName = Command.ConfigureQueueOrTopicNameArgument();
            _getInputFile = Command.ConfigureInputFileOption();
            _getInputFileEncoding = Command.ConfigureEncodingNameOption("Input file encoding", "--input-file-encoding");
            _getSendMessageEncoding = Command.ConfigureEncodingNameOption("Send message encoding", "--send-message-encoding");
        }

        protected override async Task Execute(CancellationToken cancellationToken)
        {
            await using var fileStream = await OpenFile(cancellationToken);
            var enumerator = CreateSendMessageEnumerator(fileStream);
            var print = new PrintSentMessagesCallback(Console, _getSendMessageEncoding());

            var sendBatchMessages = new SendBatchMessages(_getQueueOrTopicName(), enumerator, print.Callback);

            await Mediator.Send(sendBatchMessages, cancellationToken);
        }

        private SendMessageEnumerator CreateSendMessageEnumerator(Stream fileStream)
        {
            return new SendMessageEnumerator(new StreamReader(fileStream, _getInputFileEncoding()),
                new SendMessageParser(_getSendMessageEncoding()));
        }

        private Task<Stream> OpenFile(CancellationToken cancellationToken)
        {
            return Mediator.Send(new ReadFile(_getInputFile()), cancellationToken);
        }
    }
}