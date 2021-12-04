using Azure.Messaging.ServiceBus;

namespace ServiceBusAdmin.CommandHandlers.Models
{
    public static class MessageMapperExtensions
    {
        internal static IMessage MapToPeekedMessage(this ServiceBusReceivedMessage message)
        {
            return new Message(message.MapToMetadata(), message.Body);
        }
        
        internal static IReceivedMessage MapToReceivedMessage(this ServiceBusReceivedMessage message, ServiceBusReceiver receiver)
        {
            return new ReceivedMessageAdapter(message.MapToMetadata(), message.Body, message, receiver);
        }

        private static MessageMetadata MapToMetadata(this ServiceBusReceivedMessage message)
        {
            return new MessageMetadata(
                message.SequenceNumber,
                message.MessageId,
                message.EnqueuedTime,
                message.ExpiresAt,
                message.ApplicationProperties);
        }
    }
}