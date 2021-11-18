using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Subscription.Delete
{
    public record DeleteSubscription : IRequest
    {
        public DeleteSubscription(string topicName, string subscriptionName)
        {
            TopicName = topicName;
            SubscriptionName = subscriptionName;
        }

        public string TopicName { get; }
        public string SubscriptionName { get; }
    }
}