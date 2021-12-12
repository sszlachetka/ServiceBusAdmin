using System;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Receive
{
    public static class HandleSequenceNumbersOption
    {
        private const string Template = "-hs|--handle-sequence-numbers";

        public static Func<long[]> ConfigureHandleSequenceNumbers(this CommandLineApplication command)
        {
            var option = command.Option<string?>(
                Template,
                $"Handle messages with sequence numbers provided in form of comma-separated values. Only messages with given sequence numbers are completed. All other messages are received but not completed. Messages are received in peek-lock mode which locks them for configured lock duration (by default 1 minute). Please note that when lock is released then messages again become available for receive operation. The command will fail when particular message is received more than once. Use this option with caution when receiving messages that are not dead lettered because every receive attempt that is not completed increments delivery attempts counter and when the counter exceeds threshold then message is dead lettered. If sequence numbers are not provided, then all received messages are handled (completed).",
                CommandOptionType.SingleValue);

            return () => Convert(option.ParsedValue);
        }

        private static long[] Convert(string? value)
        {
            if (value == null) return Array.Empty<long>();

            var rawValues = value.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            return rawValues
                .Select(rawValue =>
                    long.TryParse(rawValue, out var result)
                        ? result
                        : throw new SebaCommandParsingException($"Value '{rawValue}' provided in {Template} option is invalid sequence number"))
                .ToArray();
        }
    }
}