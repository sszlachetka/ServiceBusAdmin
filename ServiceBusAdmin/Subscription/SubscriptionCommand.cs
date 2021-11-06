using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Subscription
{
    public class SubscriptionCommand : SebaCommand
    {
        public SubscriptionCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Subcommand(new PropsCommand(context, Command));
            Command.Subcommand(new PeekCommand(context, Command));
        }
    }
}