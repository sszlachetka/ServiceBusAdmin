using System.Collections.Generic;
using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Queue.List
{
    public record ListQueues : IRequest<IReadOnlyCollection<string>>;
}