using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.Subscription.Arguments;

namespace ServiceBusAdmin.Subscription
{
    public class PropsCommand : SebaCommand
    {
        private CommandArgument<string>? _argument;

        public PropsCommand(SebaContext context) : base(context)
        {
        }

        protected override void ConfigureArgsAndOptions(CommandLineApplication command)
        {
            _argument = command.ConfigureFullSubscriptionNameArgument();
        }

        protected override async Task ExecuteAsync(CommandLineApplication command, CancellationToken cancellationToken)
        {
            var (topic, subscription) = _argument.ParseFullSubscriptionName();
            var client = CreateServiceBusClient();

            var (activeMessageCount, deadLetterMessageCount) =
                await client.GetSubscriptionRuntimeProperties(topic, subscription, cancellationToken);

            Output.WriteLine($"ActiveMessageCount\t{activeMessageCount}");
            Output.WriteLine($"DeadLetterMessageCount\t{deadLetterMessageCount}");
        }
    }
}