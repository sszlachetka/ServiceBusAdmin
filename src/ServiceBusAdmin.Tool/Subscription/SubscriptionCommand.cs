using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Subscription
{
    public class SubscriptionCommand : SebaCommand
    {
        public SubscriptionCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Subcommand(new PropsCommand(context, Command));
            Command.Subcommand(new PeekCommand(context, Command));
            Command.Subcommand(new ListCommand(context, Command));
        }
    }
}