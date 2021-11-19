using System.Collections.Generic;
using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Subscription.List
{
    public record ListSubscriptions(string TopicName) : IRequest<IReadOnlyCollection<string>>;
}