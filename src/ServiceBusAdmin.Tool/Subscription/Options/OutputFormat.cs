using System;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Subscription.Options
{
    public static class OutputFormat
    {
        public static OutputFormatOption ConfigureOutputFormatOption(this CommandLineApplication command,
            string defaultValue = "{0}")
        {
            var option = command.Option<string?>(
                "-o|--output-format",
                "Index-based string format where: " +
                "0 - message body, " +
                "1 - sequence number, " +
                "2 - Id, " +
                "3 - application properties",
                CommandOptionType.SingleValue);

            return new OutputFormatOption(option, defaultValue);
        }
    }

    public class OutputFormatOption
    {
        private readonly CommandOption<string?> _option;
        private readonly string _defaultValue;

        public string Value => _option.ParsedValue ?? _defaultValue;
        public bool IncludesMessageBody => Value.Contains("{0}");
        public bool IncludesApplicationProperties => Value.Contains("{3}");

        public OutputFormatOption(CommandOption<string?> option, string defaultValue)
        {
            _option = option;
            _defaultValue = defaultValue;
        }
    }
}