using System.Collections.Generic;

namespace ServiceBusAdmin.CommandHandlers.Models
{
    public record MessageMetadata(
        long SequenceNumber,
        string MessageId,
        IReadOnlyDictionary<string, object> ApplicationProperties);
}