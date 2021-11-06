using System;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Subscription.Options
{
    public static class OutputFormat
    {
        public static CommandOption<string?> ConfigureOutputFormatOption(this CommandLineApplication command)
        {
            return command.Option<string?>(
                "-o|--output-format",
                "Provide index-based string format where: 0 - body, 1 - sequence number, 2 - Id, 3 - application properties",
                CommandOptionType.SingleValue);
        }

        public static string ParseOutputFormat(this CommandOption<string?>? option)
        {
            if (option == null) throw new ArgumentNullException(nameof(option));

            return option.ParsedValue ?? "{0}";
        }
    }
}