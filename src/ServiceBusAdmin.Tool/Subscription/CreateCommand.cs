using System;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.CommandHandlers.Subscription.Create;

namespace ServiceBusAdmin.Tool.Subscription
{
    public class CreateCommand : SebaCommand
    {
        private readonly Func<(string topic, string subscription)> _getFullSubscriptionName;

        public CreateCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Description = "Create a subscription.";
            _getFullSubscriptionName = Command.ConfigureFullSubscriptionNameArgument();
        }

        protected override Task Execute(CancellationToken cancellationToken)
        {
            var (topic, subscription) = _getFullSubscriptionName();

            var createSubscription = new CreateSubscription(topic, subscription);
            
            return Mediator.Send(createSubscription, cancellationToken);
        }
    }
}