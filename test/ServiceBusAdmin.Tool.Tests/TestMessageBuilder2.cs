using System;
using System.Collections.Generic;

namespace ServiceBusAdmin.Tool.Tests
{
    public class TestMessageBuilder2
    {
        private string _body = string.Empty;
        private long _sequenceNumber = 123;
        private string _messageId = Guid.NewGuid().ToString();
        private readonly Dictionary<string, object> _applicationProperties = new();

        public TestMessageBuilder2 WithBody(string value)
        {
            _body = value;
            return this;
        }

        public TestMessageBuilder2 WithSequenceNumber(long value)
        {
            _sequenceNumber = value;
            return this;
        }

        public TestMessageBuilder2 WithMessageId(string value)
        {
            _messageId = value;
            return this;
        }

        public TestMessageBuilder2 WithApplicationProperty(string key, object value)
        {
            _applicationProperties.Add(key, value);
            return this;
        }

        public TestMessage2 Build()
        {
            return new (new BinaryData(_body), _sequenceNumber, _messageId, _applicationProperties);
        }
    }
}