using System;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.CommandHandlers.Subscription.List;
using ServiceBusAdmin.Tool.Arguments;

namespace ServiceBusAdmin.Tool.Subscription
{
    public class ListCommand : SebaCommand
    {
        private readonly Func<string> _getTopicName;

        public ListCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Description = "List subscriptions for provided topic.";
            _getTopicName = Command.ConfigureTopicNameArgument();
        }

        protected override async Task Execute(CancellationToken cancellationToken)
        {
            var listSubscriptions = new ListSubscriptions(_getTopicName());
            var subscriptions = await Mediator.Send(listSubscriptions, cancellationToken);
            foreach (var subscription in subscriptions)
            {
                Console.Info(subscription);
            }
        }
    }
}