using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.CommandHandlers.Files;
using ServiceBusAdmin.CommandHandlers.Models;
using ServiceBusAdmin.CommandHandlers.ParseBatch;
using ServiceBusAdmin.CommandHandlers.SendBatch;
using ServiceBusAdmin.Tool.Arguments;
using ServiceBusAdmin.Tool.Options;

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
            var readFile = new ReadFile(_getInputFile(), _getInputFileEncoding());
            var fileContent = await Mediator.Send(readFile, cancellationToken);
            var messages = await Mediator.Send(new ParseMessageBatch(fileContent), cancellationToken);
            var sendBatchMessages = new SendBatchMessages(_getTopicName(), _getSendMessageEncoding(), messages);

            await Mediator.Send(sendBatchMessages, cancellationToken);
        }
    }
}