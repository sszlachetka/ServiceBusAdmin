using System;
using System.Collections.Generic;

namespace ServiceBusAdmin.CommandHandlers.Models
{
    public class SendMessageModel
    {
        public SendMessageModel(
            BinaryData body,
            SendMessageMetadataModel metadata)
            : this(body, MessageBodyFormatEnum.Unknown, metadata)
        {
        }

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