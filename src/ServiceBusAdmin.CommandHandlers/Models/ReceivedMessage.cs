using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace ServiceBusAdmin.CommandHandlers.Models
{
    public interface IReceivedMessage : IMessage
    {
        Task Complete();
        Task DeadLetter();
        Task Abandon();
    }

    internal class ReceivedMessageAdapter : IReceivedMessage
    {
        private readonly ServiceBusReceivedMessage _message;
        private readonly ServiceBusReceiver _receiver;

        public ReceivedMessageAdapter(
            MessageMetadata metadata,
            BinaryData body,
            ServiceBusReceivedMessage message,
            ServiceBusReceiver receiver)
        {
            Metadata = metadata;
            Body = body;
            _message = message;
            _receiver = receiver;
        }

        public Task Complete()
        {
            return _receiver.CompleteMessageAsync(_message);
        }

        public Task DeadLetter()
        {
            return _receiver.DeadLetterMessageAsync(_message);
        }
        
        public Task Abandon()
        {
            return _receiver.AbandonMessageAsync(_message);
        }

        public MessageMetadata Metadata { get; }
        public BinaryData Body { get; }
    }
}