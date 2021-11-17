using System;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Subscription.Options
{
    public static class MessageHandlingConcurrencyLevel
    {
        public static Func<int> ConfigureMessageHandlingConcurrencyLevel(this CommandLineApplication command, int defaultValue = 1)
        {
            var option = command.Option<int?>(
                "--message-handling-concurrency-level",
                "Maximum number of messages that can be handled concurrently.",
                CommandOptionType.SingleValue);

            return () => option.ParsedValue ?? defaultValue;
        }
    }
}