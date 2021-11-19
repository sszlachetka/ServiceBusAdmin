using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Send
{
    internal class SendMessageHandler : IRequestHandler<SendStringMessage>, IRequestHandler<SendBinaryMessage>
    {
        private readonly ServiceBusClientFactory _clientFactory;

        public SendMessageHandler(ServiceBusClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public Task<Unit> Handle(SendStringMessage request, CancellationToken cancellationToken)
        {
            var (queueOrTopicName, messageBody) = request;

            return Send(queueOrTopicName, new ServiceBusMessage(messageBody), cancellationToken);
        }

        public Task<Unit> Handle(SendBinaryMessage request, CancellationToken cancellationToken)
        {
            var (queueOrTopicName, messageBody) = request;

            return Send(queueOrTopicName, new ServiceBusMessage(messageBody), cancellationToken);
        }

        private async Task<Unit> Send(string queueOrTopicName, ServiceBusMessage message,
            CancellationToken cancellationToken)
        {
            await using var client = _clientFactory.ServiceBusClient();
            await using var sender = client.CreateSender(queueOrTopicName);

            await sender.SendMessageAsync(message, cancellationToken);

            return Unit.Value;
        }
    }
}