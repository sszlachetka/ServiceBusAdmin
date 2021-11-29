using System;
using System.Collections.Generic;
using ServiceBusAdmin.CommandHandlers;
using ServiceBusAdmin.CommandHandlers.SendBatch;

namespace ServiceBusAdmin.Tool.Tests
{
    public class SendMessageModelBuilder
    {
        private object _body = SebaSerializer.Deserialize<object>("{\"key1\":69}")!;
        private string? _messageId;
        private readonly Dictionary<string, object> _applicationProperties = new();

        public SendMessageModelBuilder WithBody(object value)
        {
            _body = value;

            return this;
        }
        
        public SendMessageModelBuilder WithMessageId(string? value)
        {
            _messageId = value;

            return this;
        }

        public SendMessageModelBuilder WithApplicationProperty(string name, object value)
        {
            _applicationProperties.Add(name, value);

            return this;
        }

        public SendMessageModelBuilder WithBody(string value)
        {
            _body = SebaSerializer.Deserialize<object>(value) ??
                    throw new InvalidOperationException("Body cannot be null");

            return this;
        }

        public SendMessageModel Build()
        {
            return new SendMessageModel(
                _body,
                new SendMessageMetadataModel(
                    _messageId,
                    _applicationProperties));
        }
    }
}