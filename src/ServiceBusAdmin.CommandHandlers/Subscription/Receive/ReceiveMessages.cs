using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Subscription.Receive
{
    public record ReceiveMessages : IRequest
    {
    }
}