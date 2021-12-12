using Azure.Messaging.ServiceBus;

namespace ServiceBusAdmin.CommandHandlers.Subscription
{
    internal static class SubscriptionReceiverFactory
    {
        public static ServiceBusReceiver Create(ServiceBusClient client, ReceiverOptions options)
        {
            var entityName = options.EntityName;
            var serviceBusReceiverOptions = new ServiceBusReceiverOptions
            {
                SubQueue = options.IsDeadLetterSubQueue ? SubQueue.DeadLetter : SubQueue.None,
                PrefetchCount = options.IsDeadLetterSubQueue ? default : 100
            };

            return entityName.IsQueue 
                ? client.CreateReceiver(entityName.QueueName(), serviceBusReceiverOptions) 
                : client.CreateReceiver(entityName.TopicName(), entityName.SubscriptionName(), serviceBusReceiverOptions);
        }
    }
}