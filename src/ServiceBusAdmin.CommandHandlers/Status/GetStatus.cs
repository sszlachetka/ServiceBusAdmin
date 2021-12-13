using System;
using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Status
{
    public record GetStatus(Action<EntityProperties> Callback) : IRequest;
}