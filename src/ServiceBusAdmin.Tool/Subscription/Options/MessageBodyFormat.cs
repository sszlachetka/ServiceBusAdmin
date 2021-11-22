using System;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.CommandHandlers.Models;

namespace ServiceBusAdmin.Tool.Subscription.Options
{
    public static class MessageBodyFormat
    {
        public static Func<MessageBodyFormatEnum> ConfigureMessageBodyFormatOption(this CommandLineApplication command,
            MessageBodyFormatEnum defaultValue = MessageBodyFormatEnum.Json)
        {
            var option = command.Option<MessageBodyFormatEnum?>(
                "-f|--message-body-format",
                "Set format of message body: \"json\" or \"text\".",
                CommandOptionType.SingleValue);

            return () => option.ParsedValue ?? defaultValue;
        }
    }
}