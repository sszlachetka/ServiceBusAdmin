using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Subscription
{
    public class SubscriptionCommand : SebaCommand
    {
        public SubscriptionCommand(SebaContext context) : base(context)
        {
        }

        protected override void ConfigureSubCommands(CommandLineApplication command)
        {
            command.Configure(new PropsCommand(Context));
            command.Configure(new PeekCommand(Context));
        }
    }
}