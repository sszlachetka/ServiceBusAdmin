using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Topic.Create
{
    public record CreateTopic : IRequest
    {
        public CreateTopic(string topicName)
        {
            TopicName = topicName;
        }

        public string TopicName { get; }
    }
}