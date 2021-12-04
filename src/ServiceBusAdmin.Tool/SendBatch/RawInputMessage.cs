using System.Collections.Generic;
using System.Text.Json;

namespace ServiceBusAdmin.Tool.SendBatch
{
    internal class RawInputMessage
    {
        public JsonElement Body { get; set; }
        public RawInputMessageMetadata? Metadata { get; set; }
    }

    internal class RawInputMessageMetadata
    {
        public string? MessageId { get; set; }
        public IReadOnlyDictionary<string, JsonElement>? ApplicationProperties { get; set; }
    }
}