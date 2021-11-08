using System;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.Tool.Subscription.Arguments;

namespace ServiceBusAdmin.Tool.Subscription
{
    public class PropsCommand : SebaCommand
    {
        private readonly Func<(string topic, string subscription)> _getFullSubscriptionName;

        public PropsCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            _getFullSubscriptionName = Command.ConfigureFullSubscriptionNameArgument();
        }

        protected override async Task Execute(CancellationToken cancellationToken)
        {
            var (topic, subscription) = _getFullSubscriptionName();

            var (activeMessageCount, deadLetterMessageCount) =
                await Client.GetSubscriptionRuntimeProperties(topic, subscription, cancellationToken);

            Console.Info($"ActiveMessageCount\t{activeMessageCount}");
            Console.Info($"DeadLetterMessageCount\t{deadLetterMessageCount}");
        }
    }
}