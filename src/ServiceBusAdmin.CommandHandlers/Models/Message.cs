using System;

namespace ServiceBusAdmin.CommandHandlers.Models
{
    public interface IMessage
    {
        public MessageMetadata Metadata { get; }
        public BinaryData Body { get; }
    }

    public class Message : IMessage
    {
        public Message(MessageMetadata metadata, BinaryData body)
        {
            Metadata = metadata;
            Body = body;
        }

        public MessageMetadata Metadata { get; }
        public BinaryData Body { get; }
    }
}