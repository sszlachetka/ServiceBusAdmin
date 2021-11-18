using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Props
{
    internal class GetNamespacePropsHandler : IRequestHandler<GetNamespaceProps, NamespaceProps>
    {
        private readonly ServiceBusClientFactory _clientFactory;

        public GetNamespacePropsHandler(ServiceBusClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<NamespaceProps> Handle(GetNamespaceProps request, CancellationToken cancellationToken)
        {
            var client = _clientFactory.AdministrationClient();
            var response = await client.GetNamespacePropertiesAsync(cancellationToken);
            var props = response.Value;

            return new NamespaceProps(props.Name, props.CreatedTime, props.ModifiedTime);
        }
    }
}