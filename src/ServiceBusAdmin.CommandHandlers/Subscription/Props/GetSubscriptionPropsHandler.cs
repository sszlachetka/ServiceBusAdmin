using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Subscription.Props
{
    internal class GetSubscriptionPropsHandler : IRequestHandler<GetSubscriptionProps, SubscriptionProps>
    {
        private readonly ServiceBusClientFactory _clientFactory;

        public GetSubscriptionPropsHandler(ServiceBusClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<SubscriptionProps> Handle(GetSubscriptionProps request, CancellationToken cancellationToken)
        {
            var client = _clientFactory.AdministrationClient();
            var response = await client.GetSubscriptionRuntimePropertiesAsync(request.TopicName,
                request.SubscriptionName, cancellationToken);
            var props = response.Value;

            return new SubscriptionProps(props.ActiveMessageCount, props.DeadLetterMessageCount);
        }
    }
}