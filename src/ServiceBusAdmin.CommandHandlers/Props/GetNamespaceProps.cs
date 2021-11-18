using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Props
{
    public record GetNamespaceProps : IRequest<NamespaceProps>
    {
    }
}