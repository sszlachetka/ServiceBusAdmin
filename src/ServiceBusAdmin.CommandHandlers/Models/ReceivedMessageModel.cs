using System.Collections.Generic;

namespace ServiceBusAdmin.CommandHandlers.Models
{
    public record ReceivedMessageModel(
            long SequenceNumber,
            string MessageId,
            IReadOnlyDictionary<string, object> ApplicationProperties,
            dynamic Body)
        : MessageMetadata(SequenceNumber, MessageId, ApplicationProperties);
}