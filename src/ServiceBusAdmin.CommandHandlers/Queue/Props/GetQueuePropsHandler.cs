using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Queue.Props
{
    internal class GetQueuePropsHandler : IRequestHandler<GetQueueProps, QueueProps>
    {
        private readonly ServiceBusClientFactory _clientFactory;

        public GetQueuePropsHandler(ServiceBusClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<QueueProps> Handle(GetQueueProps request, CancellationToken cancellationToken)
        {
            var client = _clientFactory.AdministrationClient();
            var response = await client.GetQueueRuntimePropertiesAsync(request.QueueName, cancellationToken);
            var props = response.Value;

            return new QueueProps(props.ActiveMessageCount, props.DeadLetterMessageCount);
        }
    }
}