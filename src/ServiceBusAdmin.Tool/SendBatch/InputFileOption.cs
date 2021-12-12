using System;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.SendBatch
{
    public static class InputFileOption
    {
        public static Func<string> ConfigureInputFileOption(this CommandLineApplication application)
        {
            var messageBodyOption = application
                .Option<string>("-i|--input-file",
                    $"Input file with messages to be send. Each input line must conform to JSON schema {Urls.InputMessageJsonSchema}",
                    CommandOptionType.SingleValue)
                .IsRequired();

            return () => messageBodyOption.ParsedValue;
        }
    }
}