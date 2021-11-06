using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Topic
{
    public class TopicCommand : SebaCommand
    {
        public TopicCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Subcommand(new ListCommand(Context, Command));
        }
    }
}