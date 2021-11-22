using System.Text;
using MediatR;
using ServiceBusAdmin.CommandHandlers.Models;

namespace ServiceBusAdmin.CommandHandlers.SendBatch
{
    public record SendBatchMessages
        (string QueueOrTopicName, Encoding MessageEncoding, SendMessageModel[] Messages) : IRequest;
}