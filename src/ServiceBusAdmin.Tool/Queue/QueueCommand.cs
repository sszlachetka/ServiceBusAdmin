using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Queue
{
    public class QueueCommand: SebaCommand
    {
        public QueueCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Description = "Manage your queues.";
            Command.Subcommand(new CreateCommand(context, Command));
            Command.Subcommand(new DeleteCommand(context, Command));
            Command.Subcommand(new ListCommand(context, Command));
            Command.Subcommand(new PropsCommand(context, Command));
            Command.Subcommand(new SendCommand(context, Command));
            Command.Subcommand(new SendBatchCommand(context, Command));
        }
    }
}