using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Subscription.Create
{
    internal class CreateSubscriptionHandler : IRequestHandler<CreateSubscription>
    {
        private readonly ServiceBusClientFactory _clientFactory;

        public CreateSubscriptionHandler(ServiceBusClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<Unit> Handle(CreateSubscription request, CancellationToken cancellationToken)
        {
            var client = _clientFactory.AdministrationClient();
            await client.CreateSubscriptionAsync(request.TopicName, request.SubscriptionName, cancellationToken);

            return Unit.Value;
        }
    }
}