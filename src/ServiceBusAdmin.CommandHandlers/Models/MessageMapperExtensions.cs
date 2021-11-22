using System;
using System.Text;
using System.Text.Json;
using Azure.Messaging.ServiceBus;

namespace ServiceBusAdmin.CommandHandlers.Models
{
    public static class MessageMapperExtensions
    {
        public static MessageMetadata MapToMetadata(this IMessage message)
        {
            return new MessageMetadata(
                message.SequenceNumber,
                message.MessageId,
                message.ApplicationProperties);
        }

        public static ReceivedMessageModel MapToModel(this IMessage message, Encoding encoding,
            MessageBodyFormatEnum bodyFormat)
        {
            return new ReceivedMessageModel(
                message.SequenceNumber,
                message.MessageId,
                message.ApplicationProperties,
                DeserializeBody(message.GetBodyString(encoding), bodyFormat),
                bodyFormat);
        }

        internal static ServiceBusMessage MapToServiceBusMessage(this SendMessageModel model, Encoding encoding)
        {
            var bodyString = SerializeBody(model.Body, model.BodyFormat);
            var body = encoding.GetBytes(bodyString);

            var message = new ServiceBusMessage(body);
            message.MessageId = model.MessageId;
            foreach (var (key, value) in model.ApplicationProperties)
            {
                message.ApplicationProperties.Add(key, value);
            }

            return message;
        }

        private static dynamic DeserializeBody(string bodyString, MessageBodyFormatEnum bodyFormat)
        {
            if (bodyFormat == MessageBodyFormatEnum.Json)
            {
                return JsonSerializer.Deserialize<dynamic>(bodyString)
                       ?? throw new InvalidOperationException(
                           "Message was not parsed successfully. " +
                           $"Expected format of message body '{bodyString}' was {bodyFormat}.");
            }

            if (bodyFormat == MessageBodyFormatEnum.Text) return bodyString;

            throw new ArgumentOutOfRangeException(bodyFormat.ToString());
        }

        private static string SerializeBody(dynamic body, MessageBodyFormatEnum bodyFormat)
        {
            if (bodyFormat == MessageBodyFormatEnum.Json)
            {
                return JsonSerializer.Serialize(body);
            }
            
            if (bodyFormat == MessageBodyFormatEnum.Text) return body;
            
            throw new ArgumentOutOfRangeException(bodyFormat.ToString());
        }

        public static string GetBodyString(this IMessage message, Encoding encoding)
        {
            return encoding.GetString(message.Body);
        }
    }
}