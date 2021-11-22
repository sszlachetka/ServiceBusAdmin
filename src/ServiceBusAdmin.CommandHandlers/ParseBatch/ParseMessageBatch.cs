using MediatR;
using ServiceBusAdmin.CommandHandlers.Models;

namespace ServiceBusAdmin.CommandHandlers.ParseBatch
{
    public record ParseMessageBatch(string Content) : IRequest<SendMessageModel[]>;
}