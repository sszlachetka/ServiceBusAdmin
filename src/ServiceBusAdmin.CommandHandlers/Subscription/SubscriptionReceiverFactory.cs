using Azure.Messaging.ServiceBus;

namespace ServiceBusAdmin.CommandHandlers.Subscription
{
    internal static class SubscriptionReceiverFactory
    {
        public static ServiceBusReceiver Create(ServiceBusClient client, ReceiverOptions options)
        {
            var entity = options.EntityName;
            var serviceBusReceiverOptions = new ServiceBusReceiverOptions
            {
                SubQueue = options.IsDeadLetterSubQueue ? SubQueue.DeadLetter : SubQueue.None
            };

            return client.CreateReceiver(entity.TopicName(), entity.SubscriptionName(), serviceBusReceiverOptions);
        }
    }
}