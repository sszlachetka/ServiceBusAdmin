using System;
using System.Text;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using ServiceBusAdmin.CommandHandlers.Models;

namespace ServiceBusAdmin.CommandHandlers.SendBatch
{
    internal static class SendMessageModelExtensions
    {
        internal static ServiceBusMessage MapToServiceBusMessage(this SendMessageModel model, Encoding encoding,
            MessageBodyFormatEnum bodyFormat)
        {
            var bodyString = SerializeBody(model.Body, bodyFormat);
            var body = encoding.GetBytes(bodyString);

            var message = new ServiceBusMessage(body);
            var metadata = model.Metadata;
            if (!string.IsNullOrWhiteSpace(metadata.MessageId))
                message.MessageId = metadata.MessageId;

            foreach (var (key, value) in model.Metadata.ApplicationProperties)
            {
                message.ApplicationProperties.Add(key, value);
            }

            return message;
        }

        private static string SerializeBody(dynamic body, MessageBodyFormatEnum bodyFormat)
        {
            if (bodyFormat == MessageBodyFormatEnum.Json)
            {
                return SebaSerializer.Serialize(body);
            }
            
            if (bodyFormat == MessageBodyFormatEnum.Text) return body;
            
            throw new ArgumentOutOfRangeException(bodyFormat.ToString());
        }
    }
}