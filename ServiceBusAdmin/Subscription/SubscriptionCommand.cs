using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Subscription
{
    [Command]
    [Subcommand(
        typeof(PropsCommand),
        typeof(DlqPeekCommand),
        typeof(DlqResubmitCommand)
    )]
    public class SubscriptionCommand : SbaCommandBase
    {
    }
}