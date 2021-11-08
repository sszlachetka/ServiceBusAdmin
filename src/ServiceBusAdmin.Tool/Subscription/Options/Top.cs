using System;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Subscription.Options
{
    public static class Top
    {
        public static Func<int> ConfigureTopOption(this CommandLineApplication command, string description, int defaultValue = 10)
        {
            var option = command.Option<int?>(
                "-t|--top",
                description,
                CommandOptionType.SingleValue);

            return () => option.ParsedValue ?? defaultValue;
        }
    }
}