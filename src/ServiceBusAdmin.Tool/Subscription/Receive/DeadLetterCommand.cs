using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.CommandHandlers.Models;
using ServiceBusAdmin.CommandHandlers.Subscription.Receive;
using ServiceBusAdmin.Tool.Subscription.Inputs;
using ServiceBusAdmin.Tool.Subscription.Receive.Options;

namespace ServiceBusAdmin.Tool.Subscription.Receive
{
    public class DeadLetterCommand : SebaCommand
    {
        private readonly ReceiveOptions _receiveOptions;
        private readonly SubscriptionReceiverInput _subscriptionReceiverInput;

        public DeadLetterCommand(SebaContext context,
            CommandLineApplication parentCommand) : base(context,
            parentCommand)
        {
            Command.Name = "dead-letter";
            Command.Description = "Receive messages from specified subscription and move them to the dead letter queue. " +
                                  "The command prints sequence numbers of dead lettered messages.";
            _receiveOptions = Command.ConfigureReceiveOptions();
            _subscriptionReceiverInput = new SubscriptionReceiverInput(Command, enableDeadLetterSwitch: false);
        }

        protected override async Task Execute(CancellationToken cancellationToken)
        {
            var options = _subscriptionReceiverInput.CreateReceiverOptions();
            var deadLetterMessage = new DeadLetterMessageCallback(
                _receiveOptions.CreateMessageHandlingPolicy(Console),
                Console);
            var receiveMessages = new ReceiveMessages(options, deadLetterMessage.Callback);

            await Mediator.Send(receiveMessages, cancellationToken);
        }

        private class DeadLetterMessageCallback
        {
            private readonly ReceivedMessageHandlingPolicy _handlingPolicy;
            private readonly SebaConsole _console;

            public DeadLetterMessageCallback(ReceivedMessageHandlingPolicy handlingPolicy, SebaConsole console)
            {
                _handlingPolicy = handlingPolicy;
                _console = console;
            }

            public async Task Callback(IReceivedMessage message)
            {
                if (!await _handlingPolicy.CanHandle(message))
                {
                    return;
                }

                await message.DeadLetter();
                _console.Info(message.Metadata.SequenceNumber);
            }
        }
    }
}