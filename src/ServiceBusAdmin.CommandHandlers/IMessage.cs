using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    public class PeekedMessageAdapter : IMessage
    {
        protected readonly ServiceBusReceivedMessage Message;

        public PeekedMessageAdapter(ServiceBusReceivedMessage message)
        {
            Message = message;
        }

        public BinaryData Body => Message.Body;
        public long SequenceNumber => Message.SequenceNumber;
        public string MessageId => Message.MessageId;
        public IReadOnlyDictionary<string, object> ApplicationProperties => Message.ApplicationProperties;
    }

    public class SentMessageAdapter : IMessage
    {
        private readonly ServiceBusMessage _message;

        public SentMessageAdapter(ServiceBusMessage message)
        {
            _message = message;
        }

        public BinaryData Body => _message.Body;
        public long SequenceNumber => 0;
        public string MessageId => _message.MessageId;

        public IReadOnlyDictionary<string, object> ApplicationProperties =>
            new ReadOnlyDictionary<string, object>(_message.ApplicationProperties);
    }
}