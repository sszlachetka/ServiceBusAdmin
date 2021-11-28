using System.Collections.Generic;

namespace ServiceBusAdmin.CommandHandlers.SendBatch
{
    public class SendMessageModel
    {
        public SendMessageModel(
            object body,
            SendMessageMetadataModel metadata)
        {
            Body = body;
            Metadata = metadata;
        }
        
        public object Body { get; }
        public SendMessageMetadataModel Metadata { get; }
    }

    public class SendMessageMetadataModel
    {
        public SendMessageMetadataModel(
            string? messageId = null,
            IReadOnlyDictionary<string, object>? applicationProperties = null)
        {
            MessageId = messageId;
            ApplicationProperties = applicationProperties ?? new Dictionary<string, object>();
        }

        public string? MessageId { get; }
        public IReadOnlyDictionary<string, object> ApplicationProperties { get; }
    }
}