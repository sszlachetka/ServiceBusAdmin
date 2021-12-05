using MediatR;
using ServiceBusAdmin.CommandHandlers.Models;
using ServiceBusAdmin.CommandHandlers.SendBatch;

namespace ServiceBusAdmin.CommandHandlers.Send
{
    public record SendMessage(string QueueOrTopicName, SendMessageModel Message) : IRequest;
}