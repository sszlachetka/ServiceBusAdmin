using System;
using System.Text;
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

        public static string GetBodyString(this IMessage message, Encoding encoding)
        {
            return encoding.GetString(message.Body);
        }

        public static object DeserializeBody(this IMessage message, Encoding encoding, MessageBodyFormatEnum bodyFormat)
        {
            var bodyString = message.GetBodyString(encoding);

            if (bodyFormat == MessageBodyFormatEnum.Json)
            {
                return SebaSerializer.Deserialize<object>(bodyString)
                       ?? throw new InvalidOperationException(
                           "Message was not parsed successfully. " +
                           $"Expected format of message body '{bodyString}' was {bodyFormat}.");
            }

            if (bodyFormat == MessageBodyFormatEnum.Text) return bodyString;

            throw new ArgumentOutOfRangeException(bodyFormat.ToString());
        }
    }
}