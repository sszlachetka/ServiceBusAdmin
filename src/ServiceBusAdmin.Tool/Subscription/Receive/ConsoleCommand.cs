using System;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.Client;

namespace ServiceBusAdmin.Tool.Subscription.Receive
{
    public class ConsoleCommand : SebaCommand
    {
        private readonly PrintToConsoleInput _input;

        public ConsoleCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            _input = new PrintToConsoleInput(Command);
        }

        protected override async Task Execute(CancellationToken cancellationToken)
        {
            var options = _input.CreateTopicReceiverOptions();
            var printToConsole = _input.CreatePrintToConsoleMessageHandler(Console);
            var completeMessage = new CompleteMessageHandler(printToConsole.Handle);

            await Client.Receive(options, completeMessage.Handle);
        }

        private class CompleteMessageHandler
        {
            private readonly Func<IMessage, Task> _innerHandle;

            public CompleteMessageHandler(Func<IMessage, Task> innerHandle)
            {
                _innerHandle = innerHandle;
            }

            public async Task Handle(IReceivedMessage message)
            {
                await _innerHandle(message);
                await message.Complete();
            }
        }
    }
}