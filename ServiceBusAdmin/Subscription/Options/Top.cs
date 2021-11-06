using System;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Subscription.Options
{
    public static class Top
    {
        public static CommandOption<int?> ConfigureTopOption(this CommandLineApplication command, string description)
        {
            return command.Option<int?>(
                "-t|--top",
                description,
                CommandOptionType.SingleValue);
        }

        public static int ParseTop(this CommandOption<int?>? option)
        {
            if (option == null) throw new ArgumentNullException(nameof(option));

            return option.ParsedValue ?? 10;
        }
    }
}