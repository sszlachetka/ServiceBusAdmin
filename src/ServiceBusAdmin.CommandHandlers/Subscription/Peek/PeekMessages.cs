using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Subscription.Peek
{
    public record PeekMessages(ReceiverOptions Options, MessageCallback Callback) : IRequest;
}