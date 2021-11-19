using System.Collections.Generic;

namespace ServiceBusAdmin.Tool
{
    public record MessageMetadata(
        long SequenceNumber,
        string MessageId,
        IReadOnlyDictionary<string, object> ApplicationProperties);

    public record Message(
            long SequenceNumber,
            string MessageId,
            IReadOnlyDictionary<string, object> ApplicationProperties,
            dynamic Body)
        : MessageMetadata(SequenceNumber, MessageId, ApplicationProperties);
}