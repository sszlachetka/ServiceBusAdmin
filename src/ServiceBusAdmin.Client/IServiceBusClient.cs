using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceBusAdmin.Client
{
    public interface IServiceBusClient
    {
        Task Receive(ReceiverOptions2 options, ReceivedMessageHandler2 messageHandler);

        Task SendMessage(string queueOrTopicName, BinaryData messageBody, CancellationToken cancellationToken);
    }
}