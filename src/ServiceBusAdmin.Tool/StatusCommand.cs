using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.CommandHandlers.Status;

namespace ServiceBusAdmin.Tool
{
    public class StatusCommand: SebaCommand
    {
        public StatusCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Description = "Return status of queues and subscriptions.";
        }

        protected override async Task Execute(CancellationToken cancellationToken)
        {
            var print = new PrintStatusToConsole(Console);
            await Mediator.Send(new GetStatus(print.Callback), cancellationToken);
        }

        private class PrintStatusToConsole
        {
            private readonly SebaConsole _console;

            public PrintStatusToConsole(SebaConsole console)
            {
                _console = console;
            }

            public void Callback(EntityProperties props)
            {
                _console.Info(props);
            }
        }
    }
}