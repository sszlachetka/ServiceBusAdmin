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
            Command.Description = "Peek messages from specified subscription and print them to the console.";
            _subscriptionReceiverInput = new SubscriptionReceiverInput(Command);
            _printToConsoleInput = new PrintToConsoleInput(Command);
        }

        protected override async Task Execute(CancellationToken cancellationToken)
        {
            var options = _subscriptionReceiverInput.CreateReceiverOptions();
            var print = _printToConsoleInput.CreateMessageCallback(Console);

            var peekMessages = new PeekMessages(options, print.Callback);
            
            await Mediator.Send(peekMessages, cancellationToken);
        }
    }
}