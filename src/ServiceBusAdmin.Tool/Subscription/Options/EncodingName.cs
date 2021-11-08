using System;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Subscription.Options
{
    public static class EncodingName
    {
        public static Func<string> ConfigureEncodingNameOption(this CommandLineApplication command,
            string defaultValue = "utf-8")
        {
            var option = command.Option<string?>(
                "-e|--encoding",
                "Name of encoding used to encode message body. Supported values https://docs.microsoft.com/en-us/dotnet/api/system.text.encoding?view=net-5.0",
                CommandOptionType.SingleValue);

            return () => option.ParsedValue ?? defaultValue;
        }
    }
}