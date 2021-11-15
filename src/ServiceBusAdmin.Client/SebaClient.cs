using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

namespace ServiceBusAdmin.Client
{
    public class SebaClient : IServiceBusClient
    {
        private readonly string _connectionString;

        public SebaClient(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<string> GetNamespaceName(CancellationToken cancellationToken)
        {
            var response = await AdministrationClient().GetNamespacePropertiesAsync(cancellationToken);
            var props = response.Value;

            return props.Name;
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

            var messages = await receiver.PeekMessagesAsync(options.MaxMessages);
            foreach (var message in messages)
            {
                await messageHandler(new MessageAdapter(message));
            }
        }

        public async Task Receive(ReceiverOptions options, ReceivedMessageHandler messageHandler)
        {
            await using var client = ServiceBusClient();
            await using var receiver = ServiceBusReceiver(client, options);
            
            var messages = await receiver.ReceiveMessagesAsync(options.MaxMessages);
            foreach (var message in messages)
            {
                await messageHandler(new ReceivedMessageAdapter(message, receiver));
            }
        }

        public Task CreateTopic(string topicName, CancellationToken cancellationToken)
        {
            return AdministrationClient().CreateTopicAsync(topicName, cancellationToken);
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

        public async Task SendMessage(string queueOrTopicName, string messageBody, CancellationToken cancellationToken)
        {
            await using var client = ServiceBusClient();
            await using var sender = client.CreateSender(queueOrTopicName);

            var message = new ServiceBusMessage(messageBody);

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

            return client.CreateReceiver(entity.TopicName(), entity.SubscriptionName(), new ServiceBusReceiverOptions());
        }
    }
}