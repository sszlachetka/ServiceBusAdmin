using System;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.Tool.Arguments;

namespace ServiceBusAdmin.Tool.Topic
{
    public class DeleteCommand : SebaCommand
    {
        private readonly Func<string> _getTopicName;

        public DeleteCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            _getTopicName = Command.ConfigureTopicNameArgument("Name of a topic to delete");
        }

        protected override Task Execute(CancellationToken cancellationToken)
        {
            return Client.DeleteTopic(_getTopicName(), cancellationToken);
        }
    }
}