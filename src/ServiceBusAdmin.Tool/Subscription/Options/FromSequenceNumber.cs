using System;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Subscription.Options
{
    public static class FromSequenceNumber
    {
        public static Func<long?> ConfigureFromSequenceNumber(this CommandLineApplication command)
        {
            var option = command.Option<long?>(
                "-fs|--from-sequence-number",
                "Fetch messages from specific sequence number.",
                CommandOptionType.SingleValue);

            return () => option.ParsedValue;
        }
    }
}