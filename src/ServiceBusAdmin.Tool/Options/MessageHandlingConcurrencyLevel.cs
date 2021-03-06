using System;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Options
{
    public static class MessageHandlingConcurrencyLevel
    {
        public static Func<int> ConfigureMessageHandlingConcurrencyLevel(this CommandLineApplication command, int defaultValue = 1)
        {
            var option = command.Option<int?>(
                "-cl|--message-handling-concurrency-level",
                "Maximum number of messages that can be handled concurrently.",
                CommandOptionType.SingleValue);

            return () => option.ParsedValue ?? defaultValue;
        }
    }
}