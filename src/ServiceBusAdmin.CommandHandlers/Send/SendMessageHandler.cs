using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Send
{
    internal class SendMessageHandler : IRequestHandler<SendMessage>
    {
        private readonly ServiceBusClientFactory _clientFactory;

        public SendMessageHandler(ServiceBusClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<Unit> Handle(SendMessage request, CancellationToken cancellationToken)
        {
            await using var client = _clientFactory.ServiceBusClient();
            await using var sender = client.CreateSender(request.QueueOrTopicName);

            await sender.SendMessageAsync(new ServiceBusMessage(request.MessageBody), cancellationToken);

            return Unit.Value;
        }
    }
}