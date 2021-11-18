using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Subscription.List
{
    internal class ListSubscriptionsHandler : IRequestHandler<ListSubscriptions, IReadOnlyCollection<string>>
    {
        private readonly ServiceBusClientFactory _clientFactory;

        public ListSubscriptionsHandler(ServiceBusClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IReadOnlyCollection<string>> Handle(ListSubscriptions request, CancellationToken cancellationToken)
        {
            var client = _clientFactory.AdministrationClient();
            var result = new List<string>();
            var subscriptions = client.GetSubscriptionsAsync(request.TopicName, cancellationToken);
            await foreach (var subscription in subscriptions)
            {
                result.Add(subscription.SubscriptionName);
            }

            return result;
        }
    }
}