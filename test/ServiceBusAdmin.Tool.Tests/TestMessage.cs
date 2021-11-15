using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceBusAdmin.Client;

namespace ServiceBusAdmin.Tool.Tests
{
    public class TestMessage : IReceivedMessage
    {
        private int _completeCallsCount;
        public TestMessage(BinaryData body, long sequenceNumber, string messageId,
            IReadOnlyDictionary<string, object> applicationProperties)
        {
            Body = body;
            SequenceNumber = sequenceNumber;
            MessageId = messageId;
            ApplicationProperties = applicationProperties;
        }

        public BinaryData Body { get; }
        public long SequenceNumber { get; }
        public string MessageId { get; }
        public IReadOnlyDictionary<string, object> ApplicationProperties { get; }
        public Task Complete()
        {
            _completeCallsCount++;

            return Task.CompletedTask;
        }

        public bool CompletedOnce => _completeCallsCount == 1;
    }
}