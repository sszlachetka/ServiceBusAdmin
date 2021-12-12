using MediatR;
using ServiceBusAdmin.CommandHandlers.Models;

namespace ServiceBusAdmin.CommandHandlers.Receive
{
    public record ReceiveMessages(ReceiverOptions Options, ReceivedMessageCallback Callback) : IRequest;
}