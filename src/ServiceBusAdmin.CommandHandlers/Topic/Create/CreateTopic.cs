using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Topic.Create
{
    public record CreateTopic(string TopicName) : IRequest;
}