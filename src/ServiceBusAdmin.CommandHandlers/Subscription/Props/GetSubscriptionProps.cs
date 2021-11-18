using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Subscription.Props
{
    public record GetSubscriptionProps : IRequest<SubscriptionProps>
    {
        public GetSubscriptionProps(string topicName, string subscriptionName)
        {
            TopicName = topicName;
            SubscriptionName = subscriptionName;
        }

        public string TopicName { get; }
        public string SubscriptionName { get; }
    }
}