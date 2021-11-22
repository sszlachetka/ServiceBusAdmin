using System;
using System.Text;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Options
{
    public static class EncodingName
    {
        public static Func<Encoding> ConfigureEncodingNameOption(this CommandLineApplication command,
            string description,
            string template = "-e|--encoding",
            string defaultValue = "utf-8")
        {
            var option = command.Option<string?>(
                template,
                $"{description} Supported values https://docs.microsoft.com/en-us/dotnet/api/system.text.encoding?view=net-5.0",
                CommandOptionType.SingleValue);

            return () => Encoding.GetEncoding(option.ParsedValue ?? defaultValue);
        }
    }
}