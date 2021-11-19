using System;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Subscription.Options
{
    public enum OutputContentEnum
    {
        Metadata,
        Body,
        All
    }

    public static class OutputContent
    {
        public static Func<OutputContentEnum> ConfigureOutputContentOption(this CommandLineApplication command,
            OutputContentEnum defaultValue = OutputContentEnum.Metadata)
        {
            var option = command.Option<OutputContentEnum?>(
                "-o|--output-content",
                "Set content of the output: " +
                "\"metadata\" - message metadata, " +
                "\"body\" - message body, " +
                "\"all\" - message metadata and body. " +
                "Default value is \"metadata\".",
                CommandOptionType.SingleValue);

            return () => option.ParsedValue ?? defaultValue;
        }
    }
}