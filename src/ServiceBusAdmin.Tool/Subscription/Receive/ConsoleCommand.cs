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
        private readonly Func<long[]> _handleSequenceNumbers;
        private readonly SubscriptionReceiverInput _subscriptionReceiverInput;
        private readonly PrintToConsoleInput _printToConsoleInput;

        public ConsoleCommand(
            SebaContext context,
            CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Description = "Receive messages from specified subscription and print them to the console.";
            _handleSequenceNumbers = Command.ConfigureHandleSequenceNumbers();
            _subscriptionReceiverInput = new SubscriptionReceiverInput(Command);
            _printToConsoleInput = new PrintToConsoleInput(Command);
        }

        protected override async Task Execute(CancellationToken cancellationToken)
        {
            var printToConsole = _printToConsoleInput.CreateMessageCallback(Console);
            var completeMessageDecorator = new CompleteMessageDecorator(printToConsole.Callback);
            var handleSequenceNumbersDecorator =
                new HandleSequenceNumbersDecorator(Console, _handleSequenceNumbers(), completeMessageDecorator.Callback);
            var validateDecorator = new ValidateUniqueSequenceNumberDecorator(handleSequenceNumbersDecorator.Callback);

            var options = _subscriptionReceiverInput.CreateReceiverOptions();
            var receiveMessages = new ReceiveMessages(options, validateDecorator.Callback);

            await Mediator.Send(receiveMessages, cancellationToken);
            
            validateDecorator.VerifyAllReceived(_handleSequenceNumbers());
        }

        private class CompleteMessageDecorator
        {
            private readonly Func<IMessage, Task> _innerCallback;

            public CompleteMessageDecorator(Func<IMessage, Task> innerCallback)
            {
                _innerCallback = innerCallback;
            }

            public async Task Callback(IReceivedMessage message)
            {
                await _innerCallback(message);
                await message.Complete();
            }
        }
    }
}