using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Queue.Delete
{
    public record DeleteQueue(string QueueName) : IRequest;
}