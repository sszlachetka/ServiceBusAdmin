using System;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Subscription.Options
{
    public static class MaxMessages
    {
        public static Func<int> ConfigureMaxMessagesOption(this CommandLineApplication command, string description, int defaultValue = 10)
        {
            var option = command.Option<int?>(
                "-m|--max",
                description,
                CommandOptionType.SingleValue);

            return () => option.ParsedValue ?? defaultValue;
        }
    }
}