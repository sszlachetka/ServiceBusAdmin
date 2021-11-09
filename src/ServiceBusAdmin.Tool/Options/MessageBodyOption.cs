using System;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Options
{
    public static class MessageBodyOption
    {
        public static Func<string> ConfigureMessageBodyOption(this CommandLineApplication application)
        {
            var messageBodyOption = application
                .Option<string>("-b|--body", "Message body", CommandOptionType.SingleValue)
                .IsRequired();

            return () => messageBodyOption.ParsedValue;
        }
    }
}