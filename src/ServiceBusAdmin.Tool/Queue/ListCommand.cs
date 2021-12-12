using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.CommandHandlers.Queue.List;

namespace ServiceBusAdmin.Tool.Queue
{
    public class ListCommand: SebaCommand
    {
        public ListCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Description = "List queues.";
        }

        protected override async Task Execute(CancellationToken cancellationToken)
        {
            var queues = await Mediator.Send(new ListQueues(), cancellationToken);
            foreach (var queue in queues)
            {
                Console.Info(queue);
            }
        }
    }
}