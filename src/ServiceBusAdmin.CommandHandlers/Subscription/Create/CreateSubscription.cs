using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Subscription.Create
{
    public record CreateSubscription : IRequest
    {
        public CreateSubscription(string topicName, string subscriptionName)
        {
            TopicName = topicName;
            SubscriptionName = subscriptionName;
        }

        public string TopicName { get; }
        public string SubscriptionName { get; }
    }
}