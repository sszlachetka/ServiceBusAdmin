using System;
using System.Collections.Generic;
using Azure.Messaging.ServiceBus;

namespace ServiceBusAdmin.Client
{
    public interface IMessage2
    {
        BinaryData Body { get; }
        long SequenceNumber { get; }
        string MessageId { get; }
        IReadOnlyDictionary<string, object> ApplicationProperties { get; }
    }

    public class Message2Adapter : IMessage2
    {
        protected readonly ServiceBusReceivedMessage Message;

        public Message2Adapter(ServiceBusReceivedMessage message)
        {
            Message = message;
        }

        public BinaryData Body => Message.Body;
        public long SequenceNumber => Message.SequenceNumber;
        public string MessageId => Message.MessageId;
        public IReadOnlyDictionary<string, object> ApplicationProperties => Message.ApplicationProperties;
    }
}