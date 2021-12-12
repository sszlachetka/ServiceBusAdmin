using System;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.CommandHandlers.Models;
using ServiceBusAdmin.CommandHandlers.Subscription.Receive;
using ServiceBusAdmin.Tool.Input;

namespace ServiceBusAdmin.Tool.Receive
{
    public class DeadLetterCommand : SebaCommand
    {
        private readonly Func<long[]> _handleSequenceNumbers;
        private readonly ReceiverInput _receiverInput;

        public DeadLetterCommand(SebaContext context,
            CommandLineApplication parentCommand) : base(context,
            parentCommand)
        {
            Command.Name = "dead-letter";
            Command.Description = "Receive messages from given entity and move them to the dead letter queue. " +
                                  "The command prints sequence numbers of dead lettered messages.";
            _handleSequenceNumbers = Command.ConfigureHandleSequenceNumbers();
            _receiverInput = new ReceiverInput(Command, enableDeadLetterSwitch: false);
        }

        protected override async Task Execute(CancellationToken cancellationToken)
        {
            var deadLetterMessage = new DeadLetterMessageCallback(Console);
            var handleSequenceNumbersDecorator =
                new HandleSequenceNumbersDecorator(Console, _handleSequenceNumbers(), deadLetterMessage.Callback);
            var validateDecorator = new ValidateUniqueSequenceNumberDecorator(handleSequenceNumbersDecorator.Callback);

            var options = _receiverInput.CreateReceiverOptions();
            var receiveMessages = new ReceiveMessages(options, validateDecorator.Callback);

            await Mediator.Send(receiveMessages, cancellationToken);
            
            validateDecorator.VerifyAllReceived(_handleSequenceNumbers());
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