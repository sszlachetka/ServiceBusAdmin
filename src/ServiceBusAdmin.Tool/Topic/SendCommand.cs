using System;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.CommandHandlers.Send;
using ServiceBusAdmin.Tool.Arguments;
using ServiceBusAdmin.Tool.Options;

namespace ServiceBusAdmin.Tool.Topic
{
    public class SendCommand : SebaCommand
    {
        private readonly Func<string> _getTopicName;
        private readonly Func<string> _getMessageBody;

        public SendCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            _getTopicName = Command.ConfigureTopicNameArgument();
            _getMessageBody = Command.ConfigureMessageBodyOption();
        }

        protected override Task Execute(CancellationToken cancellationToken)
        {
            var sendMessage = new SendMessage(_getTopicName(), _getMessageBody());

            return Mediator.Send(sendMessage, cancellationToken);
        }
    }
}