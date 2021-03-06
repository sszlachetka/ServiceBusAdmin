using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Subscription
{
    public class SubscriptionCommand : SebaCommand
    {
        public SubscriptionCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Description = "Manage your subscriptions.";
            Command.Subcommand(new PropsCommand(context, Command));
            Command.Subcommand(new CreateCommand(context, Command));
            Command.Subcommand(new DeleteCommand(context, Command));
            Command.Subcommand(new ListCommand(context, Command));
        }
    }
}