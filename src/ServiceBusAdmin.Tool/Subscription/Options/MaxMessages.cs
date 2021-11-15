using System;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Subscription.Options
{
    public static class MaxMessages
    {
        public static Func<int> ConfigureMaxMessagesOption(this CommandLineApplication command, int defaultValue = 10)
        {
            var option = command.Option<int?>(
                "-m|--max",
                "Maximum number of messages that will be fetched.",
                CommandOptionType.SingleValue);

            return () => option.ParsedValue ?? defaultValue;
        }
    }
}