using System;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.CommandHandlers.Subscription.Delete;
using ServiceBusAdmin.Tool.Subscription.Arguments;

namespace ServiceBusAdmin.Tool.Subscription
{
    public class DeleteCommand : SebaCommand
    {
        private readonly Func<(string topic, string subscription)> _getFullSubscriptionName;

        public DeleteCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Description = "Delete a subscription.";
            _getFullSubscriptionName = Command.ConfigureFullSubscriptionNameArgument();
        }

        protected override Task Execute(CancellationToken cancellationToken)
        {
            var (topic, subscription) = _getFullSubscriptionName();
            var deleteSubscription = new DeleteSubscription(topic, subscription);

            return Mediator.Send(deleteSubscription, cancellationToken);
        }
    }
}