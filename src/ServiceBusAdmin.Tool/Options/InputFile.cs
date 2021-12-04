using System;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Options
{
    public static class InputFile
    {
        public static Func<string> ConfigureInputFileOption(this CommandLineApplication application)
        {
            var messageBodyOption = application
                .Option<string>("-i|--input-file", "Input file", CommandOptionType.SingleValue)
                .IsRequired();

            return () => messageBodyOption.ParsedValue;
        }
    }
}