using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Topic.Create
{
    internal class CreateTopicHandler : IRequestHandler<CreateTopic>
    {
        private readonly ServiceBusClientFactory _clientFactory;

        public CreateTopicHandler(ServiceBusClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<Unit> Handle(CreateTopic request, CancellationToken cancellationToken)
        {
            var client = _clientFactory.AdministrationClient();
            await client.CreateTopicAsync(request.TopicName, cancellationToken);
            
            return Unit.Value;
        }
    }
}