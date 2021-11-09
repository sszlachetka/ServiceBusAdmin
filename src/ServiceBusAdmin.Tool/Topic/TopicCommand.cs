using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Topic
{
    public class TopicCommand : SebaCommand
    {
        public TopicCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Subcommand(new ListCommand(Context, Command));
            Command.Subcommand(new CreateCommand(Context, Command));
            Command.Subcommand(new DeleteCommand(Context, Command));
        }
    }
}