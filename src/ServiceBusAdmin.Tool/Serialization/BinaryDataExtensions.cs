using System;
using ServiceBusAdmin.CommandHandlers;
using ServiceBusAdmin.CommandHandlers.Models;

namespace ServiceBusAdmin.Tool.Serialization
{
    internal static class BinaryDataExtensions
    {
        public static object ToMessageBody(this BinaryData binaryData, MessageBodyFormatEnum bodyFormat)
        {
            return bodyFormat switch
            {
                MessageBodyFormatEnum.Json => binaryData.ToObjectFromJson<object>(SebaSerializer.Options),
                MessageBodyFormatEnum.Text => binaryData.ToString(),
                _ => throw new ArgumentOutOfRangeException(bodyFormat.ToString())
            };
        }
    }
}