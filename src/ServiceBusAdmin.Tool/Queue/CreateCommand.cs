using System;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.CommandHandlers.Queue.Create;
using ServiceBusAdmin.CommandHandlers.Subscription.Create;

namespace ServiceBusAdmin.Tool.Queue
{
    public class CreateCommand: SebaCommand
    {
        private readonly Func<string> _getQueueName;

        public CreateCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Description = "Create a queue.";
            _getQueueName = Command.ConfigureQueueNameArgument();
        }

        protected override Task Execute(CancellationToken cancellationToken)
        {
            return Mediator.Send(new CreateQueue(_getQueueName()), cancellationToken);
        }
    }
}