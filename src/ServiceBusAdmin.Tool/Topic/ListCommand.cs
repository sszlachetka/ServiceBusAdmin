using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Topic
{
    public class ListCommand: SebaCommand
    {
        public ListCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Description = "Lists all topics";
        }

        protected override async Task<SebaResult> Execute(CancellationToken cancellationToken)
        {
            var topicsNames = await Client.GetTopicsNames(cancellationToken);
            foreach (var topicName in topicsNames)
            {
                Console.WriteLine(topicName);
            }

            return SebaResult.Success;
        }
    }
}