using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Subscription.Peek
{
    public record PeekMessages(ReceiverOptions Options, MessageHandler Handler) : IRequest;
}