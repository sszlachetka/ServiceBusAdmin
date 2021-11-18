using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceBusAdmin.Client
{
    public interface IServiceBusClient
    {
        Task<IReadOnlyCollection<string>> GetSubscriptionsNames(string topicName, CancellationToken cancellationToken);

        Task<(long ActiveMessageCount, long DeadLetterMessageCount)> GetSubscriptionRuntimeProperties(
            string topic, string subscription, CancellationToken cancellationToken);

        Task Peek(ReceiverOptions options, MessageHandler messageHandler);

        Task Receive(ReceiverOptions options, ReceivedMessageHandler messageHandler);

        Task CreateSubscription(string topicName, string subscriptionName, CancellationToken cancellationToken);

        Task DeleteSubscription(string topicName, string subscriptionName, CancellationToken cancellationToken);

        Task SendMessage(string queueOrTopicName, BinaryData messageBody, CancellationToken cancellationToken);
    }
}