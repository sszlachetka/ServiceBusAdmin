using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.CommandHandlers;
using ServiceBusAdmin.CommandHandlers.Subscription.Receive;
using ServiceBusAdmin.Tool.Subscription.Inputs;

namespace ServiceBusAdmin.Tool.Subscription.Receive
{
    public class DeadLetterCommand : SebaCommand
    {
        private readonly SubscriptionReceiverInput _subscriptionReceiverInput;

        public DeadLetterCommand(SebaContext context, CommandLineApplication parentCommand) : base(context,
            parentCommand)
        {
            Command.Name = "dead-letter";
            Command.Description = "Receives messages from specified subscription and moves them to the dead letter queue. " +
                                  "The command prints sequence numbers of dead lettered messages.";
            _subscriptionReceiverInput = new SubscriptionReceiverInput(Command, enableDeadLetterSwitch: false);
        }

        protected override async Task Execute(CancellationToken cancellationToken)
        {
            var options = _subscriptionReceiverInput.CreateReceiverOptions();
            var handler = new DeadLetterMessageHandler(Console);
            var receiveMessages = new ReceiveMessages(options, handler.Handle);

            await Mediator.Send(receiveMessages, cancellationToken);
        }

        private class DeadLetterMessageHandler
        {
            private readonly SebaConsole _console;

            public DeadLetterMessageHandler(SebaConsole console)
            {
                _console = console;
            }

            public async Task Handle(IReceivedMessage message)
            {
                await message.DeadLetter();
                _console.Info(message.SequenceNumber);
            }
        }
    }
}