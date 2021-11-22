using System.Collections.Generic;
using ServiceBusAdmin.CommandHandlers.Models;

namespace ServiceBusAdmin.CommandHandlers.ParseBatch
{
    internal class RawInputMessage
    {
        public dynamic? Body { get; set; }
        public string? MessageId { get; set; }
        public MessageBodyFormatEnum? BodyFormat { get; set; }
        public IReadOnlyDictionary<string, object>? ApplicationProperties { get; set; }
    }
}