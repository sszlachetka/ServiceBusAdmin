using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Topic.Delete
{
    public record DeleteTopic(string TopicName) : IRequest;
}