using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Subscription.Receive
{
    public class ReceiveCommand : SebaCommand
    {
        public ReceiveCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Description = "Receive messages from specified subscription and handle them with sub-command.";
            Command.Subcommand(new ConsoleCommand(context, Command));
            Command.Subcommand(new DeadLetterCommand(context, Command));
            Command.Subcommand(new SendToTopicCommand(context, Command));
        }
    }
}