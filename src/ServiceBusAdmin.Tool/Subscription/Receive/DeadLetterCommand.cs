using System;
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
        private readonly Func<long[]> _handleSequenceNumbers;
        private readonly SubscriptionReceiverInput _subscriptionReceiverInput;

        public DeadLetterCommand(SebaContext context,
            CommandLineApplication parentCommand) : base(context,
            parentCommand)
        {
            Command.Name = "dead-letter";
            Command.Description = "Receive messages from given subscription and move them to the dead letter queue. " +
                                  "The command prints sequence numbers of dead lettered messages.";
            _handleSequenceNumbers = Command.ConfigureHandleSequenceNumbers();
            _subscriptionReceiverInput = new SubscriptionReceiverInput(Command, enableDeadLetterSwitch: false);
        }

        protected override async Task Execute(CancellationToken cancellationToken)
        {
            var deadLetterMessage = new DeadLetterMessageCallback(Console);
            var handleSequenceNumbersDecorator =
                new HandleSequenceNumbersDecorator(Console, _handleSequenceNumbers(), deadLetterMessage.Callback);
            var validateDecorator = new ValidateUniqueSequenceNumberDecorator(handleSequenceNumbersDecorator.Callback);

            var options = _subscriptionReceiverInput.CreateReceiverOptions();
            var receiveMessages = new ReceiveMessages(options, validateDecorator.Callback);

            await Mediator.Send(receiveMessages, cancellationToken);
        }

        private class DeadLetterMessageCallback
        {
            private readonly SebaConsole _console;

            public DeadLetterMessageCallback(SebaConsole console)
            {
                _console = console;
            }

            public async Task Callback(IReceivedMessage message)
            {
                await message.DeadLetter();
                _console.Info(message.Metadata.SequenceNumber);
            }
        }
    }
}