using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Subscription.Receive
{
    public class ReceiveCommand : SebaCommand
    {
        public ReceiveCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Subcommand(new ConsoleCommand(context, Command));
        }
    }
}