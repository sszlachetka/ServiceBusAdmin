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
    public class ConsoleCommand : SebaCommand
    {
        private readonly ReceiveOptions _receiveOptions;
        private readonly SubscriptionReceiverInput _subscriptionReceiverInput;
        private readonly PrintToConsoleInput _printToConsoleInput;

        public ConsoleCommand(
            SebaContext context,
            CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Description = "Receive messages from specified subscription and print them to the console.";
            _receiveOptions = Command.ConfigureReceiveOptions();
            _subscriptionReceiverInput = new SubscriptionReceiverInput(Command);
            _printToConsoleInput = new PrintToConsoleInput(Command);
        }

        protected override async Task Execute(CancellationToken cancellationToken)
        {
            var options = _subscriptionReceiverInput.CreateReceiverOptions();
            var printToConsole = _printToConsoleInput.CreateMessageCallback(Console);
            var completeMessage = new CompleteMessageCallback(
                _receiveOptions.CreateMessageHandlingPolicy(Console),
                printToConsole.Callback);

            var receiveMessages = new ReceiveMessages(options, completeMessage.Callback);

            await Mediator.Send(receiveMessages, cancellationToken);
        }

        private class CompleteMessageCallback
        {
            private readonly ReceivedMessageHandlingPolicy _handlingPolicy;
            private readonly Func<IMessage, Task> _innerHandle;

            public CompleteMessageCallback(ReceivedMessageHandlingPolicy handlingPolicy,
                Func<IMessage, Task> innerHandle)
            {
                _handlingPolicy = handlingPolicy;
                _innerHandle = innerHandle;
            }

            public async Task Callback(IReceivedMessage message)
            {
                if (!await _handlingPolicy.CanHandle(message))
                {
                    return;
                }

                await _innerHandle(message);
                await message.Complete();
            }
        }
    }
}