using System.Collections.Generic;

namespace ServiceBusAdmin.CommandHandlers.SendBatch
{
    internal class RawInputMessage
    {
        public dynamic? Body { get; set; }
        public RawInputMessageMetadata? Metadata { get; set; }
    }

    internal class RawInputMessageMetadata
    {
        public string? MessageId { get; set; }
        public IReadOnlyDictionary<string, object>? ApplicationProperties { get; set; }
    }
}