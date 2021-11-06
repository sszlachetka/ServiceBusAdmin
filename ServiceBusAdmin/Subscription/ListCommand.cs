using System;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.Subscription.Arguments;

namespace ServiceBusAdmin.Subscription
{
    public class ListCommand : SebaCommand
    {
        private readonly Func<string> _getTopicName;

        public ListCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            _getTopicName = Command.ConfigureTopicNameArgument();
        }

        protected override async Task<SebaResult> Execute(CancellationToken cancellationToken)
        {
            var subscriptions = await CreateClient().GetSubscriptionsNames(_getTopicName(), cancellationToken);
            foreach (var subscription in subscriptions)
            {
                Output.WriteLine(subscription);
            }

            return SebaResult.Success;
        }
    }
}