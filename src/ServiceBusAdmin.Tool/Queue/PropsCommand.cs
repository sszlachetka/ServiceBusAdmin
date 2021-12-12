using System;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.CommandHandlers.Queue.Props;

namespace ServiceBusAdmin.Tool.Queue
{
    public class PropsCommand : SebaCommand
    {
        private readonly Func<string> _getQueueName;

        public PropsCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Description = "Return queue properties.";
            _getQueueName = Command.ConfigureQueueNameArgument();
        }

        protected override async Task Execute(CancellationToken cancellationToken)
        {
            var queueProps = await Mediator.Send(new GetQueueProps(_getQueueName()), cancellationToken);

            Console.Info(queueProps);
        }
    }
}