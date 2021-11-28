using System.Collections.Generic;
using ServiceBusAdmin.CommandHandlers.Models;

namespace ServiceBusAdmin.CommandHandlers.SendBatch
{
    internal class RawInputMessage
    {
        public dynamic? Body { get; set; }
        public string? MessageId { get; set; }
        public IReadOnlyDictionary<string, object>? ApplicationProperties { get; set; }
    }
}