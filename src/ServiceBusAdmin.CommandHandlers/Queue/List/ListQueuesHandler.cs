using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Queue.List
{
    internal class ListQueuesHandler : IRequestHandler<ListQueues, IReadOnlyCollection<string>>
    {
        private readonly ServiceBusClientFactory _clientFactory;

        public ListQueuesHandler(ServiceBusClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IReadOnlyCollection<string>> Handle(ListQueues request, CancellationToken cancellationToken)
        {
            var client = _clientFactory.AdministrationClient();
            var result = new List<string>();
            var queues = client.GetQueuesAsync(cancellationToken);
            
            await foreach (var queue in queues)
            {
                result.Add(queue.Name);
            }

            return result;
        }
    }
}