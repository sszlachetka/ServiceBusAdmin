using System;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.CommandHandlers.Topic.Delete;
using ServiceBusAdmin.Tool.Arguments;

namespace ServiceBusAdmin.Tool.Topic
{
    public class DeleteCommand : SebaCommand
    {
        private readonly Func<string> _getTopicName;

        public DeleteCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Description = "Delete a topic.";
            _getTopicName = Command.ConfigureTopicNameArgument("Name of a topic to delete");
        }

        protected override Task Execute(CancellationToken cancellationToken)
        {
            return Mediator.Send(new DeleteTopic(_getTopicName()), cancellationToken);
        }
    }
}