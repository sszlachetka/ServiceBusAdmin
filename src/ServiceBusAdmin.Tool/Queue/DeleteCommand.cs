using System;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.CommandHandlers.Queue.Delete;

namespace ServiceBusAdmin.Tool.Queue
{
    public class DeleteCommand : SebaCommand
    {
        private readonly Func<string> _getQueueName;

        public DeleteCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Description = "Delete given queue.";
            _getQueueName = Command.ConfigureQueueNameArgument();
        }

        protected override Task Execute(CancellationToken cancellationToken)
        {
            return Mediator.Send(new DeleteQueue(_getQueueName()), cancellationToken);
        }
    }
}