using System;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Peek
{
    public static class FromSequenceNumberOption
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