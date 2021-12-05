using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.CommandHandlers.Send;
using ServiceBusAdmin.Tool.Arguments;
using ServiceBusAdmin.Tool.MessageParsing;
using ServiceBusAdmin.Tool.Options;

namespace ServiceBusAdmin.Tool.Topic
{
    public class SendCommand : SebaCommand
    {
        private readonly Func<string> _getTopicName;
        private readonly Func<string> _getMessage;
        private readonly Func<Encoding> _getMessageBodyEncoding;

        public SendCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Description = "Send new message to a topic.";
            _getTopicName = Command.ConfigureTopicNameArgument();
            _getMessage = Command.ConfigureInputMessageOption();
            _getMessageBodyEncoding =
                Command.ConfigureEncodingNameOption("Name of encoding used to encode message body.");
        }

        protected override Task Execute(CancellationToken cancellationToken)
        {
            var messageParser = new SendMessageParser(_getMessageBodyEncoding());
            var message = messageParser.Parse(_getMessage());
            
            var sendMessage = new SendMessage(_getTopicName(), message);

            return Mediator.Send(sendMessage, cancellationToken);
        }
    }
}