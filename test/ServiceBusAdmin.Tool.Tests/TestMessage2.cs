using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceBusAdmin.Client;

namespace ServiceBusAdmin.Tool.Tests
{
    public class TestMessage2 : IReceivedMessage2
    {
        private int _completeCallsCount;
        private int _deadLetterCallsCount;

        public TestMessage2(BinaryData body, long sequenceNumber, string messageId,
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
        public bool CompletedOnce => _completeCallsCount == 1;
        public bool DeadLetteredOnce => _deadLetterCallsCount == 1;

        public Task Complete()
        {
            _completeCallsCount++;

            return Task.CompletedTask;
        }

        public Task DeadLetter()
        {
            _deadLetterCallsCount++;

            return Task.CompletedTask;
        }
    }
}