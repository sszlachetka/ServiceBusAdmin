using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Send
{
    public record SendStringMessage(string QueueOrTopicName, string MessageBody) : IRequest;
}