using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceBusAdmin.CommandHandlers;
using ServiceBusAdmin.CommandHandlers.Models;

namespace ServiceBusAdmin.Tool.Tests
{
    public class TestMessage : IReceivedMessage
    {
        private int _completeCallsCount;
        private int _deadLetterCallsCount;

        public TestMessage(MessageMetadata metadata, BinaryData body)
        {
            Metadata = metadata;
            Body = body;
        }

        public MessageMetadata Metadata { get; }
        public BinaryData Body { get; }
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