using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Queue.Create
{
    internal class CreateQueueHandler : IRequestHandler<CreateQueue>
    {
        private readonly ServiceBusClientFactory _clientFactory;

        public CreateQueueHandler(ServiceBusClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<Unit> Handle(CreateQueue request, CancellationToken cancellationToken)
        {
            var client = _clientFactory.AdministrationClient();
            await client.CreateQueueAsync(request.QueueName, cancellationToken);

            return Unit.Value;
        }
    }
}