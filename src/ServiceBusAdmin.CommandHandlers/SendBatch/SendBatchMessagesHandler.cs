using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ServiceBusAdmin.CommandHandlers.Models;

namespace ServiceBusAdmin.CommandHandlers.SendBatch
{
    internal class SendBatchMessagesHandler : IRequestHandler<SendBatchMessages>
    {
        private readonly ServiceBusClientFactory _clientFactory;

        public SendBatchMessagesHandler(ServiceBusClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<Unit> Handle(SendBatchMessages request, CancellationToken cancellationToken)
        {
            var (queueOrTopicName, encoding, messages) = request;
            
            await using var client = _clientFactory.ServiceBusClient();
            await using var sender = client.CreateSender(queueOrTopicName);

            var index = 0;
            do
            {
                var batch = await sender.CreateMessageBatchAsync(cancellationToken);
                var startBatchIndex = index;
                while (index < messages.Length && batch.TryAddMessage(messages[index].MapToServiceBusMessage(encoding))) index++;
                if (startBatchIndex == index)
                {
                    throw new ApplicationException(
                        $"Failed to create message batch with message {JsonSerializer.Serialize(messages[index])}");
                }

                await sender.SendMessagesAsync(batch, cancellationToken);
            } while (index < messages.Length);

            return Unit.Value;
        }
    }
}