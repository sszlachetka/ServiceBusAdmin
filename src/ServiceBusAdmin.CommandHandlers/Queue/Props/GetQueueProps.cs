using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Queue.Props
{
    public record GetQueueProps(string QueueName) : IRequest<QueueProps>;
}