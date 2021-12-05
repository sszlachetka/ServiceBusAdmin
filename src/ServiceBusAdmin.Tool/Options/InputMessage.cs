using System;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Options
{
    public static class InputMessage
    {
        public static Func<string> ConfigureInputMessageOption(this CommandLineApplication application)
        {
            var messageBodyOption = application
                .Option<string>("-m|--message", $"JSON message that conforms to schema {Urls.InputMessageJsonSchema}",
                    CommandOptionType.SingleValue)
                .IsRequired();

            return () => messageBodyOption.ParsedValue;
        }
    }
}