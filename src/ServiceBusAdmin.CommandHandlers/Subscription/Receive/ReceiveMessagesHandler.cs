using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Subscription.Receive
{
    internal class ReceiveMessagesHandler : IRequestHandler<ReceiveMessages>
    {
        private static readonly TimeSpan ReceiveMaxWaitTime = TimeSpan.FromSeconds(3);
        private readonly ServiceBusClientFactory _clientFactory;

        public ReceiveMessagesHandler(ServiceBusClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<Unit> Handle(ReceiveMessages request, CancellationToken cancellationToken)
        {
            await using var client = _clientFactory.ServiceBusClient();
            var (options, receivedMessageHandler) = request;
            await using var receiver = SubscriptionReceiverFactory.Create(client, options);

            var receivedCount = 0;
            IReadOnlyList<ServiceBusReceivedMessage> messages;
            do
            {
                messages = await receiver.ReceiveMessagesAsync(options.MaxMessages - receivedCount,
                    ReceiveMaxWaitTime, cancellationToken);
                receivedCount += messages.Count;
                var receivedMessages = messages.Select(m => new ReceivedMessageAdapter(m, receiver)).ToList();
                await HandleReceivedMessages(receivedMessages, receivedMessageHandler, options.MessageHandlingConcurrencyLevel);
            } while (messages.Count > 0 && receivedCount < options.MaxMessages);
            
            return Unit.Value;
        }
        
        private static async Task HandleReceivedMessages(
            IReadOnlyCollection<IReceivedMessage> messages,
            ReceivedMessageHandler messageHandler,
            int messageHandlingConcurrencyLevel)
        {
            if (messages.Count == 0) return;

            var semaphore = new SemaphoreSlim(messageHandlingConcurrencyLevel);
            var tasks = new List<Task>();
            foreach (var message in messages)
            {
                await semaphore.WaitAsync();

                tasks.Add(messageHandler(message)
                    .ContinueWith(_ => semaphore.Release()));
            }
            
            await Task.WhenAll(tasks);
        }
    }
}