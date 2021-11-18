using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace ServiceBusAdmin.Client
{
    public class SebaClient : IServiceBusClient
    {
        private static readonly TimeSpan ReceiveMaxWaitTime = TimeSpan.FromSeconds(3);
        private readonly string _connectionString;

        public SebaClient(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task Peek(ReceiverOptions options, MessageHandler messageHandler)
        {
            await using var client = ServiceBusClient();
            await using var receiver = ServiceBusReceiver(client, options);

            var peekedCount = 0;
            IReadOnlyList<ServiceBusReceivedMessage> messages;
            do
            {
                messages = await receiver.PeekMessagesAsync(options.MaxMessages - peekedCount);
                peekedCount += messages.Count;
                var peekedMessages = messages.Select(m => new MessageAdapter(m)).ToList();
                await HandlePeekedMessages(peekedMessages, messageHandler, options.MessageHandlingConcurrencyLevel);
            } while (messages.Count > 0 && peekedCount < options.MaxMessages);
        }
        
        private static async Task HandlePeekedMessages(
            IReadOnlyCollection<IMessage> messages,
            MessageHandler messageHandler,
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

        public async Task Receive(ReceiverOptions options, ReceivedMessageHandler messageHandler)
        {
            await using var client = ServiceBusClient();
            await using var receiver = ServiceBusReceiver(client, options);

            var receivedCount = 0;
            IReadOnlyList<ServiceBusReceivedMessage> messages;
            do
            {
                messages = await receiver.ReceiveMessagesAsync(options.MaxMessages - receivedCount, ReceiveMaxWaitTime);
                receivedCount += messages.Count;
                var receivedMessages = messages.Select(m => new ReceivedMessageAdapter(m, receiver)).ToList();
                await HandleReceivedMessages(receivedMessages, messageHandler, options.MessageHandlingConcurrencyLevel);
            } while (messages.Count > 0 && receivedCount < options.MaxMessages);
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

        public Task SendMessage(string queueOrTopicName, BinaryData messageBody, CancellationToken cancellationToken)
        {
            return SendMessage(queueOrTopicName, new ServiceBusMessage(messageBody), cancellationToken);
        }
        
        private async Task SendMessage(string queueOrTopicName, ServiceBusMessage message, CancellationToken cancellationToken)
        {
            await using var client = ServiceBusClient();
            await using var sender = client.CreateSender(queueOrTopicName);

            await sender.SendMessageAsync(message, cancellationToken);
        }

        private ServiceBusClient ServiceBusClient()
        {
            return new(_connectionString);
        }
        
        private static ServiceBusReceiver ServiceBusReceiver(ServiceBusClient client, ReceiverOptions options)
        {
            var entity = options.EntityName;
            var serviceBusReceiverOptions = new ServiceBusReceiverOptions
            {
                SubQueue = options.IsDeadLetterSubQueue ? SubQueue.DeadLetter : SubQueue.None
            };

            return client.CreateReceiver(entity.TopicName(), entity.SubscriptionName(), serviceBusReceiverOptions);
        }
    }
}