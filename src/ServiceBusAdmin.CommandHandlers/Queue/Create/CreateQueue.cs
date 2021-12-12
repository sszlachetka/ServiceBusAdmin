using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Queue.Create
{
    public record CreateQueue(string QueueName) : IRequest;
}