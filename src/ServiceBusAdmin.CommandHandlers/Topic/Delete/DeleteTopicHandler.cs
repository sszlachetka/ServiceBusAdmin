using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Topic.Delete
{
    internal class DeleteTopicHandler : IRequestHandler<DeleteTopic>
    {
        private readonly ServiceBusClientFactory _clientFactory;

        public DeleteTopicHandler(ServiceBusClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<Unit> Handle(DeleteTopic request, CancellationToken cancellationToken)
        {
            var client = _clientFactory.AdministrationClient();
            await client.DeleteTopicAsync(request.TopicName, cancellationToken);
            
            return Unit.Value;
        }
    }
}