using System;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Subscription.Options
{
    public static class OutputFormat
    {
        public static Func<string> ConfigureOutputFormatOption(this CommandLineApplication command,
            string defaultValue = "{0}")
        {
            var option = command.Option<string?>(
                "-o|--output-format",
                "Provide index-based string format where: 0 - body, 1 - sequence number, 2 - Id, 3 - application properties",
                CommandOptionType.SingleValue);

            return () => option.ParsedValue ?? defaultValue;
        }
    }
}