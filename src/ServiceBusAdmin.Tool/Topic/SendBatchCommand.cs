using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.CommandHandlers.Models;
using ServiceBusAdmin.CommandHandlers.SendBatch;
using ServiceBusAdmin.Tool.Arguments;
using ServiceBusAdmin.Tool.Options;
using ServiceBusAdmin.Tool.Subscription.Options;

namespace ServiceBusAdmin.Tool.Topic
{
    public class SendBatchCommand : SebaCommand
    {
        private readonly Func<string> _getTopicName;
        private readonly Func<string> _getInputFile;
        private readonly Func<Encoding> _getInputFileEncoding;
        private readonly Func<Encoding> _getSendMessageEncoding;
        private readonly Func<MessageBodyFormatEnum> _messageBodyFormat;
        

        public SendBatchCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Name = "send-batch";
            Command.Description = "Send batch of messages to a topic.";
            _getTopicName = Command.ConfigureTopicNameArgument();
            _getInputFile = Command.ConfigureInputFileOption();
            _getInputFileEncoding = Command.ConfigureEncodingNameOption("Input file encoding", "--input-file-encoding");
            _getSendMessageEncoding = Command.ConfigureEncodingNameOption("Send message encoding", "--send-message-encoding");
            _messageBodyFormat = Command.ConfigureMessageBodyFormatOption();
        }

        protected override async Task Execute(CancellationToken cancellationToken)
        {
            var readMessages = new ReadMessages(_getInputFile(), _getInputFileEncoding());
            var messages = await Mediator.Send(readMessages, cancellationToken);
            await using var messagesEnumerator = messages.GetAsyncEnumerator(cancellationToken);
            var printHandler = new PrintToConsoleMessageHandler(_messageBodyFormat(), OutputContentEnum.All,
                _getSendMessageEncoding(), Console);

            var sendBatchMessages = new SendBatchMessages(_getTopicName(), _getSendMessageEncoding(),
                _messageBodyFormat(), messagesEnumerator, printHandler.Handle);

            await Mediator.Send(sendBatchMessages, cancellationToken);
        }
    }
}