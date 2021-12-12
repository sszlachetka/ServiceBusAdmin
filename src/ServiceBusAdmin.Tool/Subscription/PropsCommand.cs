using System;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.CommandHandlers.Subscription.Props;

namespace ServiceBusAdmin.Tool.Subscription
{
    public class PropsCommand : SebaCommand
    {
        private readonly Func<(string topic, string subscription)> _getFullSubscriptionName;

        public PropsCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Description = "Return subscription properties.";
            _getFullSubscriptionName = Command.ConfigureFullSubscriptionNameArgument();
        }

        protected override async Task Execute(CancellationToken cancellationToken)
        {
            var (topic, subscription) = _getFullSubscriptionName();
            var getSubscriptionProperties = new GetSubscriptionProps(topic, subscription);

            var subscriptionProps = await Mediator.Send(getSubscriptionProperties, cancellationToken);

            Console.Info(subscriptionProps);
        }
    }
}