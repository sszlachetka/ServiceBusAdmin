using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceBusAdmin.Client
{
    public interface IServiceBusClient
    {
        Task Peek(ReceiverOptions options, MessageHandler messageHandler);

        Task Receive(ReceiverOptions options, ReceivedMessageHandler messageHandler);

        Task SendMessage(string queueOrTopicName, BinaryData messageBody, CancellationToken cancellationToken);
    }
}