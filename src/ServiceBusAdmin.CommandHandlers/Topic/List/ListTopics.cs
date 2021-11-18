using System.Collections.Generic;
using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Topic.List
{
    public record ListTopics : IRequest<IReadOnlyCollection<string>>
    {
    }
}