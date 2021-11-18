using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Subscription.Delete
{
    internal class DeleteSubscriptionHandler : IRequestHandler<DeleteSubscription>
    {
        private readonly ServiceBusClientFactory _clientFactory;

        public DeleteSubscriptionHandler(ServiceBusClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<Unit> Handle(DeleteSubscription request, CancellationToken cancellationToken)
        {
            var client = _clientFactory.AdministrationClient();
            await client.DeleteSubscriptionAsync(request.TopicName, request.SubscriptionName, cancellationToken);

            return Unit.Value;
        }
    }
}