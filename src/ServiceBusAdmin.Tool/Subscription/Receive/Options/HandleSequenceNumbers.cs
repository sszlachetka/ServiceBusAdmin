using System;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Subscription.Receive.Options
{
    public static class HandleSequenceNumbers
    {
        public const string Template = "-hs|--handle-sequence-numbers";

        public static Func<long[]> ConfigureHandleSequenceNumbers(this CommandLineApplication command)
        {
            var option = command.Option<string?>(
                Template,
                $"Handle messages with provided sequence numbers. Expected format of sequence numbers are comma-separated values. Only messages with given sequence numbers are completed. All other messages are received but not completed. Messages are received in peek-lock mode, which means that not completed messages are locked for configured time duration (by default 1 minute). If not completed messages must be explicitly abandoned, then use {AbandonUnhandledMessages.Template} option. If sequence numbers are not provided, then all messages are handled (completed).",
                CommandOptionType.SingleValue,
                inherited: true);

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