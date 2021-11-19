using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Subscription.Receive
{
    public record ReceiveMessages(ReceiverOptions Options, ReceivedMessageHandler Handler) : IRequest;
}