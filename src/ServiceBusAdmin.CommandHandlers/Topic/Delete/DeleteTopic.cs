using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Topic.Delete
{
    public record DeleteTopic : IRequest
    {
        public DeleteTopic(string topicName)
        {
            TopicName = topicName;
        }

        public string TopicName { get; }
    }
}