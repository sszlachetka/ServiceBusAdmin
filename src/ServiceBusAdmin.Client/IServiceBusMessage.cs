using System;
using System.Collections.Generic;
using Azure.Messaging.ServiceBus;

namespace ServiceBusAdmin.Client
{
    public interface IServiceBusMessage
    {
        BinaryData Body { get; }
        long SequenceNumber { get; }
        string MessageId { get; }
        IReadOnlyDictionary<string, object> ApplicationProperties { get; }
    }

    public class ServiceBusReceivedMessageAdapter : IServiceBusMessage
    {
        private readonly ServiceBusReceivedMessage _message;

        public ServiceBusReceivedMessageAdapter(ServiceBusReceivedMessage message)
        {
            _message = message;
        }

        public BinaryData Body => _message.Body;
        public long SequenceNumber => _message.SequenceNumber;
        public string MessageId => _message.MessageId;
        public IReadOnlyDictionary<string, object> ApplicationProperties => _message.ApplicationProperties;
    }
}