using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using MediatR;
using ServiceBusAdmin.CommandHandlers.Models;
using ServiceBusAdmin.CommandHandlers.Subscription;

namespace ServiceBusAdmin.CommandHandlers.Receive
{
    internal class ReceiveMessagesHandler : IRequestHandler<ReceiveMessages>
    {
        private const int MaxReceivePageSize = 250; 
        private static readonly TimeSpan ReceiveMaxWaitTime = TimeSpan.FromSeconds(3);
        private readonly ServiceBusClientFactory _clientFactory;

        public ReceiveMessagesHandler(ServiceBusClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<Unit> Handle(ReceiveMessages request, CancellationToken cancellationToken)
        {
            var (receiverOptions, receivedMessageCallback) = request;
            var toReceiveCount = receiverOptions.MaxMessages;
            var receivedCount = 0;
            while (receivedCount < toReceiveCount)
            {
                var notReceivedCount = toReceiveCount - receivedCount;
                var pageSize = notReceivedCount > MaxReceivePageSize ? MaxReceivePageSize : notReceivedCount;

                await Receive(new ReceiveMessages(receiverOptions with { MaxMessages = pageSize }, receivedMessageCallback),
                    cancellationToken);

                receivedCount += pageSize;
            }

            return Unit.Value;
        }

        private async Task Receive(ReceiveMessages request, CancellationToken cancellationToken)
        {
            await using var client = _clientFactory.ServiceBusClient();
            var (options, receivedMessageHandler) = request;
            await using var receiver = ReceiverFactory.Create(client, options);

            var receivedCount = 0;
            IReadOnlyList<ServiceBusReceivedMessage> messages;
            do
            {
                messages = await receiver.ReceiveMessagesAsync(options.MaxMessages - receivedCount,
                    ReceiveMaxWaitTime, cancellationToken);

                if (messages.Count == 0) continue;

                receivedCount += messages.Count;
                var receivedMessages = messages.Select(m => m.MapToReceivedMessage(receiver)).ToList();
                await HandleReceivedMessages(receivedMessages, receivedMessageHandler, options.MessageHandlingConcurrencyLevel);
            } while (messages.Count > 0 && receivedCount < options.MaxMessages);
        }

        private static async Task HandleReceivedMessages(
            IReadOnlyCollection<IReceivedMessage> messages,
            ReceivedMessageCallback messageCallback,
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