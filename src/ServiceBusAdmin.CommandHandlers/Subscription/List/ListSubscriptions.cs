using System.Collections.Generic;
using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Subscription.List
{
    public record ListSubscriptions : IRequest<IReadOnlyCollection<string>>
    {
        public ListSubscriptions(string topicName)
        {
            TopicName = topicName;
        }

        public string TopicName { get; }
    }
}