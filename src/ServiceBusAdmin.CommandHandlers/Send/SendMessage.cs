using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Send
{
    public record SendMessage(string QueueOrTopicName, string MessageBody) : IRequest;
}