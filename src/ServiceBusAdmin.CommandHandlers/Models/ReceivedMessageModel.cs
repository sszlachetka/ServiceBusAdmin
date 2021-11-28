using System;
using System.Collections.Generic;

namespace ServiceBusAdmin.CommandHandlers.Models
{
    public record ReceivedMessageModel(
            long SequenceNumber,
            string MessageId,
            DateTimeOffset EnqueuedTime,
            IReadOnlyDictionary<string, object> ApplicationProperties,
            dynamic Body)
        : MessageMetadata(SequenceNumber, MessageId, EnqueuedTime, ApplicationProperties);
}