using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace ServiceBusAdmin.Client
{
    public interface IServiceBusClient
    {
        Task<string> GetNamespaceName(CancellationToken cancellationToken);

        Task<IReadOnlyCollection<string>> GetTopicsNames(CancellationToken cancellationToken);

        Task<IReadOnlyCollection<string>> GetSubscriptionsNames(string topicName, CancellationToken cancellationToken);

        Task<(long ActiveMessageCount, long DeadLetterMessageCount)> GetSubscriptionRuntimeProperties(
            string topic, string subscription, CancellationToken cancellationToken);

        Task Peek(TopicReceiverOptions options, Func<ServiceBusReceivedMessage, Task> messageHandler);

        Task CreateTopic(string topicName, CancellationToken cancellationToken);

        Task DeleteTopic(string topicName, CancellationToken cancellationToken);
    }
}