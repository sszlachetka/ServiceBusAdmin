using System;
using System.Collections.Generic;
using ServiceBusAdmin.CommandHandlers.Models;

namespace ServiceBusAdmin.CommandHandlers.SendBatch
{
    public class SendMessageModel
    {
        public SendMessageModel(
            BinaryData body,
            MessageBodyFormatEnum bodyFormat,
            SendMessageMetadataModel metadata)
        {
            Body = body;
            BodyFormat = bodyFormat;
            Metadata = metadata;
        }
        
        public BinaryData Body { get; }
        public MessageBodyFormatEnum BodyFormat { get; }
        public SendMessageMetadataModel Metadata { get; }
    }

    public class SendMessageMetadataModel
    {
        public SendMessageMetadataModel(
            string? messageId = null,
            IReadOnlyDictionary<string, object>? applicationProperties = null)
        {
            MessageId = messageId;
            ApplicationProperties = applicationProperties ?? new Dictionary<string, object>();
        }

        public string? MessageId { get; }
        public IReadOnlyDictionary<string, object> ApplicationProperties { get; }
    }
}