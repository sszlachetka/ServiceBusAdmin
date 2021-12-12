using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Queue.Delete
{
    internal class DeleteQueueHandler : IRequestHandler<DeleteQueue>
    {
        private readonly ServiceBusClientFactory _clientFactory;

        public DeleteQueueHandler(ServiceBusClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<Unit> Handle(DeleteQueue request, CancellationToken cancellationToken)
        {
            var client = _clientFactory.AdministrationClient();
            await client.DeleteQueueAsync(request.QueueName, cancellationToken);
            
            return Unit.Value;
        }
    }
}