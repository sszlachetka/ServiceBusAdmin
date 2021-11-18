using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.CommandHandlers.Topic.List;

namespace ServiceBusAdmin.Tool.Topic
{
    public class ListCommand: SebaCommand
    {
        public ListCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Description = "Lists all topics";
        }

        protected override async Task Execute(CancellationToken cancellationToken)
        {
            var topicsNames = await Mediator.Send(new ListTopics(), cancellationToken);
            foreach (var topicName in topicsNames)
            {
                Console.Info(topicName);
            }
        }
    }
}