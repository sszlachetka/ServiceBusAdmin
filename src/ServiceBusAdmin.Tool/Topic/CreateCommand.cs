using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Topic
{
    public class CreateCommand : SebaCommand
    {
        private readonly CommandArgument<string> _topicNameArg;

        public CreateCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            _topicNameArg = Command.Argument<string>("Topic name", "Name of a topic to create").IsRequired();
        }

        protected override Task Execute(CancellationToken cancellationToken)
        {
            var topicName = _topicNameArg.ParsedValue;

            return Client.CreateTopic(topicName, cancellationToken);
        }
    }
}