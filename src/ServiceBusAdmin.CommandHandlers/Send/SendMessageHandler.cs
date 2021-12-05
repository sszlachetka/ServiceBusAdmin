using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ServiceBusAdmin.CommandHandlers.Models;

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
            var (queueOrTopicName, message) = request;
            
            await using var client = _clientFactory.ServiceBusClient();
            await using var sender = client.CreateSender(queueOrTopicName);

            await sender.SendMessageAsync(message.MapToServiceBusMessage(), cancellationToken);

            return Unit.Value;
        }
    }
}