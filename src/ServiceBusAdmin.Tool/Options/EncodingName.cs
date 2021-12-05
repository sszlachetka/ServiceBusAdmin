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
                $"{description} Supported values {Urls.SupportedEncodingNames}",
                CommandOptionType.SingleValue);

            return () => Encoding.GetEncoding(option.ParsedValue ?? defaultValue);
        }
    }
}