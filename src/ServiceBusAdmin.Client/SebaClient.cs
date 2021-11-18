using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

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

        public async Task<IReadOnlyCollection<string>> GetTopicsNames(CancellationToken cancellationToken)
        {
            var result = new List<string>();
            var topics = AdministrationClient().GetTopicsAsync(cancellationToken);
            await foreach (var topic in topics)
            {
                result.Add(topic.Name);
            }

            return result;
        }
        
        public async Task<IReadOnlyCollection<string>> GetSubscriptionsNames(string topicName, CancellationToken cancellationToken)
        {
            var result = new List<string>();
            var subscriptions = AdministrationClient().GetSubscriptionsAsync(topicName, cancellationToken);
            await foreach (var subscription in subscriptions)
            {
                result.Add(subscription.SubscriptionName);
            }

            return result;
        }

        public async Task<(long ActiveMessageCount, long DeadLetterMessageCount)> GetSubscriptionRuntimeProperties(
            string topic, string subscription, CancellationToken cancellationToken)
        {
            var response = await AdministrationClient()
                .GetSubscriptionRuntimePropertiesAsync(topic, subscription, cancellationToken);
            var props = response.Value;

            return (props.ActiveMessageCount, props.DeadLetterMessageCount);
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

        public Task DeleteTopic(string topicName, CancellationToken cancellationToken)
        {
            return AdministrationClient().DeleteTopicAsync(topicName, cancellationToken);
        }

        public Task CreateSubscription(string topicName, string subscriptionName,
            CancellationToken cancellationToken)
        {
            return AdministrationClient().CreateSubscriptionAsync(topicName, subscriptionName, cancellationToken);
        }

        public Task DeleteSubscription(string topicName, string subscriptionName,
            CancellationToken cancellationToken)
        {
            return AdministrationClient().DeleteSubscriptionAsync(topicName, subscriptionName, cancellationToken);
        }

        public Task SendMessage(string queueOrTopicName, string messageBody, CancellationToken cancellationToken)
        {
            return SendMessage(queueOrTopicName, new ServiceBusMessage(messageBody), cancellationToken);
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

        private ServiceBusAdministrationClient AdministrationClient()
        {
            return new (_connectionString);
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