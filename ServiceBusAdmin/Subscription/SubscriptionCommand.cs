using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Subscription
{
    [Command]
    [Subcommand(
        typeof(PropsCommand),
        typeof(PeekCommand),
        typeof(DlqPeekCommand),
        typeof(DlqResubmitCommand)
    )]
    public class SubscriptionCommand : SbaCommandBase
    {
    }
}