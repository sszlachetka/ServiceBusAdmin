using MediatR;
using ServiceBusAdmin.CommandHandlers.Models;

namespace ServiceBusAdmin.CommandHandlers.Peek
{
    public record PeekMessages(ReceiverOptions Options, MessageCallback Callback, long? FromSequenceNumber) : IRequest;
}