using System;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.Tool.Subscription.Arguments;

namespace ServiceBusAdmin.Tool.Subscription
{
    public class CreateCommand : SebaCommand
    {
        private readonly Func<(string topic, string subscription)> _getFullSubscriptionName;

        public CreateCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            _getFullSubscriptionName = Command.ConfigureFullSubscriptionNameArgument();
        }

        protected override Task Execute(CancellationToken cancellationToken)
        {
            var (topic, subscription) = _getFullSubscriptionName();

            return Client.CreateSubscription(topic, subscription, cancellationToken);
        }
    }
}