using System;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Options
{
    public static class MaxMessages
    {
        public static Func<int> ConfigureMaxMessagesOption(this CommandLineApplication command,
            string? description = null,
            int defaultValue = 10)
        {
            var option = command.Option<int?>(
                "-m|--max",
                description ?? "Maximum number of messages that can be received.",
                CommandOptionType.SingleValue);

            return () => option.ParsedValue ?? defaultValue;
        }
    }
}