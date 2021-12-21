using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using MediatR;
using ServiceBusAdmin.CommandHandlers.Models;
using ServiceBusAdmin.CommandHandlers.Subscription;

namespace ServiceBusAdmin.CommandHandlers.Peek
{
    internal class PeekMessagesHandler : IRequestHandler<PeekMessages>
    {
        private readonly ServiceBusClientFactory _clientFactory;

        public PeekMessagesHandler(ServiceBusClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<Unit> Handle(PeekMessages request, CancellationToken cancellationToken)
        {
            var (options, messageHandler, fromSequenceNumber) = request;
            await using var client = _clientFactory.ServiceBusClient();
            await using var receiver = ReceiverFactory.Create(client, options);

            var peekedCount = 0;
            IReadOnlyList<ServiceBusReceivedMessage> messages;
            var fromSeq = fromSequenceNumber;
            do
            {
                messages = await receiver.PeekMessagesAsync(options.MaxMessages - peekedCount, fromSeq,
                    cancellationToken);

                if (messages.Count == 0) continue;

                fromSeq = messages[^1].SequenceNumber + 1;
                peekedCount += messages.Count;
                var peekedMessages = messages.Select(m => m.MapToPeekedMessage()).ToList();
                await HandlePeekedMessages(peekedMessages, messageHandler, options.MessageHandlingConcurrencyLevel);
            } while (messages.Count > 0 && peekedCount < options.MaxMessages);

            return Unit.Value;
        }

        private static async Task HandlePeekedMessages(
            IReadOnlyCollection<IMessage> messages,
            MessageCallback messageCallback,
            int messageHandlingConcurrencyLevel)
        {
            if (messages.Count == 0) return;

            var semaphore = new SemaphoreSlim(messageHandlingConcurrencyLevel);
            var tasks = new List<Task>();
            foreach (var message in messages)
            {
                await semaphore.WaitAsync();

                tasks.Add(messageCallback(message)
                    .ContinueWith(_ => semaphore.Release()));
            }
            
            await Task.WhenAll(tasks);
        }
    }
}