using System;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.CommandHandlers;
using ServiceBusAdmin.CommandHandlers.Subscription.Receive;
using ServiceBusAdmin.Tool.Subscription.Inputs;

namespace ServiceBusAdmin.Tool.Subscription.Receive
{
    public class ConsoleCommand : SebaCommand
    {
        private readonly SubscriptionReceiverInput _subscriptionReceiverInput;
        private readonly PrintToConsoleInput _printToConsoleInput;

        public ConsoleCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Description = "Receive messages from specified subscription and print them to the console.";
            _subscriptionReceiverInput = new SubscriptionReceiverInput(Command);
            _printToConsoleInput = new PrintToConsoleInput(Command);
        }

        protected override async Task Execute(CancellationToken cancellationToken)
        {
            var options = _subscriptionReceiverInput.CreateReceiverOptions();
            var printToConsole = _printToConsoleInput.CreateMessageCallback(Console);
            var completeMessage = new CompleteMessageCallback(printToConsole.Callback);

            var receiveMessages = new ReceiveMessages(options, completeMessage.Callback);

            await Mediator.Send(receiveMessages, cancellationToken);
        }

        private class CompleteMessageCallback
        {
            private readonly Func<IMessage, Task> _innerHandle;

            public CompleteMessageCallback(Func<IMessage, Task> innerHandle)
            {
                _innerHandle = innerHandle;
            }

            public async Task Callback(IReceivedMessage message)
            {
                await _innerHandle(message);
                await message.Complete();
            }
        }
    }
}