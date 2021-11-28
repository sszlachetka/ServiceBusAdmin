using System;
using System.Collections.Generic;

namespace ServiceBusAdmin.CommandHandlers.Models
{
    public class MessageMetadata
    {
        public MessageMetadata(long sequenceNumber,
            string messageId,
            DateTimeOffset enqueuedTime,
            DateTimeOffset expiresAt,
            IReadOnlyDictionary<string, object> applicationProperties)
        {
            SequenceNumber = sequenceNumber;
            MessageId = messageId;
            EnqueuedTime = enqueuedTime;
            ExpiresAt = expiresAt;
            ApplicationProperties = applicationProperties;
        }

        public long SequenceNumber { get; }
        public string MessageId { get; }
        public DateTimeOffset EnqueuedTime { get; }
        public DateTimeOffset ExpiresAt { get; }
        public IReadOnlyDictionary<string, object> ApplicationProperties { get; }
    }
}