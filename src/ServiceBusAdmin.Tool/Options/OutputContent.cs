using System;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Options
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
                $"Set content of the output to message metadata, body of the message or both at a time. Default value is {OutputContentEnum.Metadata}.",
                CommandOptionType.SingleValue);

            return () => option.ParsedValue ?? defaultValue;
        }
    }
}