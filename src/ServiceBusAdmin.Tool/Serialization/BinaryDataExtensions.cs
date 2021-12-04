using System;
using System.Text;
using ServiceBusAdmin.CommandHandlers;
using ServiceBusAdmin.CommandHandlers.Models;

namespace ServiceBusAdmin.Tool.Serialization
{
    internal static class BinaryDataExtensions
    {
        public static object ToMessageBody(this BinaryData binaryData, Encoding encoding,
            MessageBodyFormatEnum bodyFormat)
        {
            var bodyString = binaryData.ToMessageBodyString(encoding);

            return bodyFormat switch
            {
                MessageBodyFormatEnum.Json => SebaSerializer.Deserialize<object>(bodyString)
                                              ?? throw new InvalidOperationException($"Message body was deserialized to null."),
                MessageBodyFormatEnum.Text => bodyString,
                _ => throw new ArgumentOutOfRangeException(bodyFormat.ToString())
            };
        }

        public static string ToMessageBodyString(this BinaryData binaryData, Encoding encoding)
        {
            return encoding.GetString(binaryData);
        }
    }
}