using System;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.Tool.Subscription.Arguments;

namespace ServiceBusAdmin.Tool.Subscription
{
    public class ListCommand : SebaCommand
    {
        private readonly Func<string> _getTopicName;

        public ListCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            _getTopicName = Command.ConfigureTopicNameArgument();
        }

        protected override async Task Execute(CancellationToken cancellationToken)
        {
            var subscriptions = await Client.GetSubscriptionsNames(_getTopicName(), cancellationToken);
            foreach (var subscription in subscriptions)
            {
                Console.Info(subscription);
            }
        }
    }
}