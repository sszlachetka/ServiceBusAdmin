using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Status
{
    internal class GetStatusHandler : IRequestHandler<GetStatus>
    {
        private readonly ServiceBusClientFactory _clientFactory;

        public GetStatusHandler(ServiceBusClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<Unit> Handle(GetStatus request, CancellationToken cancellationToken)
        {
            await GetQueuesStatus(request.Callback, cancellationToken);
            await GetTopicsStatus(request.Callback, cancellationToken);

            return Unit.Value;
        }

        private async Task GetQueuesStatus(Action<EntityProperties> callback, CancellationToken cancellationToken)
        {
            var client = _clientFactory.AdministrationClient();
            var queues = client.GetQueuesAsync(cancellationToken);
            await foreach (var queue in queues)
            {
                var response = await client.GetQueueRuntimePropertiesAsync(queue.Name, cancellationToken);
                var props = response.Value;
                callback(new EntityProperties(
                    EntityType.Queue, 
                    queue.Name, 
                    props.ActiveMessageCount,
                    props.DeadLetterMessageCount));
            }
        }
        
        private async Task GetTopicsStatus(Action<EntityProperties> callback, CancellationToken cancellationToken)
        {
            var client = _clientFactory.AdministrationClient();
            var topics = client.GetTopicsAsync(cancellationToken);
            await foreach (var topic in topics)
            {
                await GetSubscriptionsStatus(topic.Name, callback, cancellationToken);
            }
        }
        
        private async Task GetSubscriptionsStatus(string topicName, Action<EntityProperties> callback,
            CancellationToken cancellationToken)
        {
            var client = _clientFactory.AdministrationClient();
            var subscriptions = client.GetSubscriptionsAsync(topicName, cancellationToken);
            await foreach (var subscription in subscriptions)
            {
                var response = await client.GetSubscriptionRuntimePropertiesAsync(topicName,
                    subscription.SubscriptionName, cancellationToken);
                var props = response.Value;
                callback(new EntityProperties(
                    EntityType.Subscription, 
                    $"{topicName}/{subscription.SubscriptionName}", 
                    props.ActiveMessageCount,
                    props.DeadLetterMessageCount));
            }
        }
    }
}