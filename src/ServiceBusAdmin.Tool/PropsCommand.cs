using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.CommandHandlers.Props;

namespace ServiceBusAdmin.Tool
{
    public class PropsCommand : SebaCommand
    {
        public PropsCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Description = "Returns topic properties.";
        }

        protected override async Task Execute(CancellationToken cancellationToken)
        {
            var props = await Mediator.Send(new GetNamespaceProps(), cancellationToken);

            Console.Info(props);
        }
    }
}