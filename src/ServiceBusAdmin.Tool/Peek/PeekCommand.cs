using System;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.CommandHandlers.Peek;
using ServiceBusAdmin.Tool.Input;

namespace ServiceBusAdmin.Tool.Peek
{
    public class PeekCommand : SebaCommand
    {
        private readonly ReceiverInput _receiverInput;
        private readonly PrintToConsoleInput _printToConsoleInput;
        private readonly Func<long?> _getFromSequenceNumber;

        public PeekCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Description = "Peek messages from given entity and print them to the console.";
            _receiverInput =
                new ReceiverInput(Command, "Maximum number of messages that can be peeked.");
            _printToConsoleInput = new PrintToConsoleInput(Command);
            _getFromSequenceNumber = Command.ConfigureFromSequenceNumber();
        }

        protected override async Task Execute(CancellationToken cancellationToken)
        {
            var options = _receiverInput.CreateReceiverOptions();
            var print = _printToConsoleInput.CreateMessageCallback(Console);

            var peekMessages = new PeekMessages(options, print.Callback, _getFromSequenceNumber());
            
            await Mediator.Send(peekMessages, cancellationToken);
        }
    }
}