using System;
using System.Collections.Generic;

namespace ServiceBusAdmin.CommandHandlers.Models
{
    public record MessageMetadata(
        long SequenceNumber,
        string MessageId,
        DateTimeOffset EnqueuedTime,
        IReadOnlyDictionary<string, object> ApplicationProperties);
}