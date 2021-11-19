using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Subscription.Delete
{
    public record DeleteSubscription(string TopicName, string SubscriptionName) : IRequest;
}