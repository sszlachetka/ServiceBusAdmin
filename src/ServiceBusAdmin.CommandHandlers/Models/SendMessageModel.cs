using System.Collections.Generic;

namespace ServiceBusAdmin.CommandHandlers.Models
{
    public class SendMessageModel
    {
        public SendMessageModel(
            dynamic body,
            string? messageId = null,
            MessageBodyFormatEnum? bodyFormat = null,
            IReadOnlyDictionary<string, object>? applicationProperties = null)
        {
            MessageId = messageId;
            Body = body;
            BodyFormat = bodyFormat ?? MessageBodyFormatEnum.Json;
            ApplicationProperties = applicationProperties ?? new Dictionary<string, object>();
        }

        public string? MessageId { get; }
        public dynamic Body { get; }
        public MessageBodyFormatEnum BodyFormat { get; }
        public IReadOnlyDictionary<string, object> ApplicationProperties { get; }

    }
}