using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.CommandHandlers.Subscription.Peek;
using ServiceBusAdmin.Tool.Subscription.Inputs;

namespace ServiceBusAdmin.Tool.Subscription
{
    public class PeekCommand : SebaCommand
    {
        private readonly SubscriptionReceiverInput _subscriptionReceiverInput;
        private readonly PrintToConsoleInput _printToConsoleInput;

        public PeekCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Description = "Peeks messages from specified subscription and prints them to the console.";
            _subscriptionReceiverInput = new SubscriptionReceiverInput(Command);
            _printToConsoleInput = new PrintToConsoleInput(Command);
        }

        protected override async Task Execute(CancellationToken cancellationToken)
        {
            var options = _subscriptionReceiverInput.CreateReceiverOptions();
            var messageHandler = _printToConsoleInput.CreateMessageHandler(Console);

            var peekMessages = new PeekMessages(options, messageHandler.Handle);
            
            await Mediator.Send(peekMessages, cancellationToken);
        }
    }
}