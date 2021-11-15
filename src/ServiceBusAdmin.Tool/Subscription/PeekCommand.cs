using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Subscription
{
    public class PeekCommand : SebaCommand
    {
        private readonly PrintToConsoleInput _input;

        public PeekCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            _input = new PrintToConsoleInput(Command);
        }

        protected override async Task Execute(CancellationToken cancellationToken)
        {
            var options = _input.CreateTopicReceiverOptions();
            var messageHandler = _input.CreatePrintToConsoleMessageHandler(Console);

            await Client.Peek(options, messageHandler.Handle);
        }
    }
}