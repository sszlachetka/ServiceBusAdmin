using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using ServiceBusAdmin.CommandHandlers.Models;

namespace ServiceBusAdmin.CommandHandlers.SendBatch
{
    public delegate Task MessagesSentCallback(IReadOnlyCollection<SendMessageModel> messages);
    
    public record SendBatchMessages(
        string QueueOrTopicName,
        IAsyncEnumerator<SendMessageModel> MessageEnumerator,
        MessagesSentCallback MessagesSentCallback) : IRequest;
}