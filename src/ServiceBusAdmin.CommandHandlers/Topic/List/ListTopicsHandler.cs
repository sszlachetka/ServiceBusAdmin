using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Topic.List
{
    internal class ListTopicsHandler : IRequestHandler<ListTopics, IReadOnlyCollection<string>>
    {
        private readonly ServiceBusClientFactory _clientFactory;

        public ListTopicsHandler(ServiceBusClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IReadOnlyCollection<string>> Handle(ListTopics request, CancellationToken cancellationToken)
        {
            var client = _clientFactory.AdministrationClient();
            var result = new List<string>();
            var topics = client.GetTopicsAsync(cancellationToken);
            await foreach (var topic in topics)
            {
                result.Add(topic.Name);
            }

            return result;
        }
    }
}