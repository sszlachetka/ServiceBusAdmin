using System.Collections.Generic;

namespace ServiceBusAdmin.CommandHandlers.SendBatch
{
    public class SendMessageModel
    {
        public SendMessageModel(
            dynamic body,
            string? messageId = null,
            IReadOnlyDictionary<string, object>? applicationProperties = null)
        {
            MessageId = messageId;
            Body = body;
            ApplicationProperties = applicationProperties ?? new Dictionary<string, object>();
        }

        public string? MessageId { get; }
        public dynamic Body { get; }
        public IReadOnlyDictionary<string, object> ApplicationProperties { get; }

    }
}