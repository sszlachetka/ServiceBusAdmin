using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Subscription.Create
{
    public record CreateSubscription(string TopicName, string SubscriptionName) : IRequest;
}