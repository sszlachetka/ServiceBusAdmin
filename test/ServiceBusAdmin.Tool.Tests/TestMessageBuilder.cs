using System;
using System.Collections.Generic;
using ServiceBusAdmin.CommandHandlers.Models;

namespace ServiceBusAdmin.Tool.Tests
{
    public class TestMessageBuilder
    {
        private string _body = string.Empty;
        private long _sequenceNumber = 123;
        private string _messageId = Guid.NewGuid().ToString();
        private DateTimeOffset _enqueuedTime = new(2005, 7, 22, 16, 34, 59, TimeSpan.Zero);
        private DateTimeOffset _expiresAt = new(2009, 2, 15, 13, 54, 21, TimeSpan.Zero);
        private readonly Dictionary<string, object> _applicationProperties = new();

        public TestMessageBuilder WithBody(string value)
        {
            _body = value;
            return this;
        }

        public TestMessageBuilder WithSequenceNumber(long value)
        {
            _sequenceNumber = value;
            return this;
        }

        public TestMessageBuilder WithMessageId(string value)
        {
            _messageId = value;
            return this;
        }
        
        public TestMessageBuilder WithEnqueuedTime(DateTimeOffset value)
        {
            _enqueuedTime = value;
            return this;
        }
        
        public TestMessageBuilder WithExpiresAt(DateTimeOffset value)
        {
            _expiresAt = value;
            return this;
        }

        public TestMessageBuilder WithApplicationProperty(string key, object value)
        {
            _applicationProperties.Add(key, value);
            return this;
        }

        public TestMessage Build()
        {
            return new(
                new MessageMetadata(_sequenceNumber, _messageId, _enqueuedTime, _expiresAt, _applicationProperties),
                new BinaryData(_body));
        }
    }
}