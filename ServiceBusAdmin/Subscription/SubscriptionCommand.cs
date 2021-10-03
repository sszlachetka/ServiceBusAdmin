using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Subscription
{
    [Command]
    [Subcommand(
        typeof(PropsCommand),
        typeof(PeekDlqCommand))]
    public class SubscriptionCommand : SbaCommandBase
    {
    }
}