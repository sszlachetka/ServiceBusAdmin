using System;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Subscription.Options
{
    public static class EncodingName
    {
        public static CommandOption<string?> ConfigureEncodingNameOption(this CommandLineApplication command)
        {
            return command.Option<string?>(
                "-e|--encoding",
                "Name of encoding used to encode message body. Supported values https://docs.microsoft.com/en-us/dotnet/api/system.text.encoding?view=net-5.0",
                CommandOptionType.SingleValue);
        }

        public static string ParseEncodingName(this CommandOption<string?>? option)
        {
            if (option == null) throw new ArgumentNullException(nameof(option));

            return option.ParsedValue ?? "utf-8";
        }
    }
}