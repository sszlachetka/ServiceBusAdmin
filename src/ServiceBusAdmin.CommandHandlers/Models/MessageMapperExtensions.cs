using System;
using System.Text;

namespace ServiceBusAdmin.CommandHandlers.Models
{
    public static class MessageMapperExtensions
    {
        public static MessageMetadata MapToMetadata(this IMessage message)
        {
            return new MessageMetadata(
                message.SequenceNumber,
                message.MessageId,
                message.EnqueuedTime,
                message.ApplicationProperties);
        }

        public static ReceivedMessageModel MapToModel(this IMessage message, Encoding encoding,
            MessageBodyFormatEnum bodyFormat)
        {
            return new ReceivedMessageModel(
                message.SequenceNumber,
                message.MessageId,
                message.EnqueuedTime,
                message.ApplicationProperties,
                DeserializeBody(message.GetBodyString(encoding), bodyFormat));
        }

        private static dynamic DeserializeBody(string bodyString, MessageBodyFormatEnum bodyFormat)
        {
            if (bodyFormat == MessageBodyFormatEnum.Json)
            {
                return SebaSerializer.Deserialize<dynamic>(bodyString)
                       ?? throw new InvalidOperationException(
                           "Message was not parsed successfully. " +
                           $"Expected format of message body '{bodyString}' was {bodyFormat}.");
            }

            if (bodyFormat == MessageBodyFormatEnum.Text) return bodyString;

            throw new ArgumentOutOfRangeException(bodyFormat.ToString());
        }

        public static string GetBodyString(this IMessage message, Encoding encoding)
        {
            return encoding.GetString(message.Body);
        }
    }
}