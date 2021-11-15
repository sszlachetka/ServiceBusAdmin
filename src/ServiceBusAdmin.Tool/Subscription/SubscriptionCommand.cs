using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.Tool.Subscription.Receive;

namespace ServiceBusAdmin.Tool.Subscription
{
    public class SubscriptionCommand : SebaCommand
    {
        public SubscriptionCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Subcommand(new PropsCommand(context, Command));
            Command.Subcommand(new CreateCommand(context, Command));
            Command.Subcommand(new DeleteCommand(context, Command));
            Command.Subcommand(new PeekCommand(context, Command));
            Command.Subcommand(new ListCommand(context, Command));
            Command.Subcommand(new ReceiveCommand(context, Command));
        }
    }
}