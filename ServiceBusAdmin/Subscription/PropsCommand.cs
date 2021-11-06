using System;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.Subscription.Arguments;

namespace ServiceBusAdmin.Subscription
{
    public class PropsCommand : SebaCommand
    {
        private readonly Func<(string topic, string subscription)> _getFullSubscriptionName;

        public PropsCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            _getFullSubscriptionName = Command.ConfigureFullSubscriptionNameArgument();
        }

        protected override async Task<SebaResult> Execute(CancellationToken cancellationToken)
        {
            var (topic, subscription) = _getFullSubscriptionName();
            var client = CreateServiceBusClient();

            var (activeMessageCount, deadLetterMessageCount) =
                await client.GetSubscriptionRuntimeProperties(topic, subscription, cancellationToken);

            Output.WriteLine($"ActiveMessageCount\t{activeMessageCount}");
            Output.WriteLine($"DeadLetterMessageCount\t{deadLetterMessageCount}");

            return SebaResult.Success;
        }
    }
}