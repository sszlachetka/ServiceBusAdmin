using System.Collections.Generic;
using System.Text;
using MediatR;
using ServiceBusAdmin.CommandHandlers.Models;

namespace ServiceBusAdmin.CommandHandlers.SendBatch
{
    public record SendBatchMessages(
        string QueueOrTopicName,
        Encoding MessageEncoding,
        MessageBodyFormatEnum BodyFormat,
        IAsyncEnumerator<SendMessageModel> MessageEnumerator,
        MessageHandler Handler) : IRequest;
}