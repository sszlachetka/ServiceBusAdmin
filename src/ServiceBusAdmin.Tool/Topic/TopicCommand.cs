using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Topic
{
    public class TopicCommand : SebaCommand
    {
        public TopicCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Description = "Manage your topics.";
            Command.Subcommand(new CreateCommand(Context, Command));
            Command.Subcommand(new DeleteCommand(Context, Command));
            Command.Subcommand(new ListCommand(Context, Command));
            Command.Subcommand(new SendCommand(Context, Command));
            Command.Subcommand(new SendBatchCommand(Context, Command));
        }
    }
}