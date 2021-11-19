using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Subscription.Props
{
    public record GetSubscriptionProps(string TopicName, string SubscriptionName) : IRequest<SubscriptionProps>;
}