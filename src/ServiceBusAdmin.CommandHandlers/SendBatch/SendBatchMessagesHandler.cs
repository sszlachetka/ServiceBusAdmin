using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using MediatR;

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
            var (queueOrTopicName, encoding, bodyFormat, messages, callback) = request;
            
            await using var client = _clientFactory.ServiceBusClient();
            await using var sender = client.CreateSender(queueOrTopicName);

            var readNext = await messages.MoveNextAsync();
            do
            {
                var batchMessages = new List<SendMessageModel>();
                using var batch = await sender.CreateMessageBatchAsync(cancellationToken);
                while (readNext)
                {
                    var message = messages.Current.MapToServiceBusMessage(encoding, bodyFormat);
                    if (!batch.TryAddMessage(message)) break;

                    batchMessages.Add(messages.Current);
                    readNext = await messages.MoveNextAsync();
                }

                await sender.SendMessagesAsync(batch, cancellationToken);
                await callback(batchMessages);
            } while (readNext);

            return Unit.Value;
        }
    }
}