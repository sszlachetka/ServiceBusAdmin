using System;
using System.Collections.Generic;
using Azure.Messaging.ServiceBus;

namespace ServiceBusAdmin.CommandHandlers
{
    public interface IMessage
    {
        BinaryData Body { get; }
        long SequenceNumber { get; }
        string MessageId { get; }
        IReadOnlyDictionary<string, object> ApplicationProperties { get; }
    }

    public class MessageAdapter : IMessage
    {
        protected readonly ServiceBusReceivedMessage Message;

        public MessageAdapter(ServiceBusReceivedMessage message)
        {
            Message = message;
        }

        public BinaryData Body => Message.Body;
        public long SequenceNumber => Message.SequenceNumber;
        public string MessageId => Message.MessageId;
        public IReadOnlyDictionary<string, object> ApplicationProperties => Message.ApplicationProperties;
    }
}