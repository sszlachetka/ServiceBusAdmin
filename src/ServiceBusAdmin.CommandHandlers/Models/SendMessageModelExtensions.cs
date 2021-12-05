using Azure.Messaging.ServiceBus;

namespace ServiceBusAdmin.CommandHandlers.Models
{
    internal static class SendMessageModelExtensions
    {
        internal static ServiceBusMessage MapToServiceBusMessage(this SendMessageModel model)
        {
            var message = new ServiceBusMessage(model.Body);
            var metadata = model.Metadata;
            if (!string.IsNullOrWhiteSpace(metadata.MessageId))
                message.MessageId = metadata.MessageId;

            foreach (var (key, value) in model.Metadata.ApplicationProperties)
            {
                message.ApplicationProperties.Add(key, value);
            }

            return message;
        }
    }
}