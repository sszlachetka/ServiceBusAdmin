using MediatR;
using ServiceBusAdmin.CommandHandlers.Models;

namespace ServiceBusAdmin.CommandHandlers.Subscription.Peek
{
    public record PeekMessages(ReceiverOptions Options, MessageCallback Callback) : IRequest;
}