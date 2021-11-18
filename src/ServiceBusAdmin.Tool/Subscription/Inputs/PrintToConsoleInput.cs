using System;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.Client;
using ServiceBusAdmin.Tool.Subscription.Arguments;
using ServiceBusAdmin.Tool.Subscription.Options;

namespace ServiceBusAdmin.Tool.Subscription.Inputs
{
    internal class PrintToConsoleInput
    {
        private readonly OutputFormatOption _outputFormat;
        private readonly Func<string> _getEncodingName;

        public PrintToConsoleInput(CommandLineApplication command)
        {
            _outputFormat = command.ConfigureOutputFormatOption();
            _getEncodingName = command.ConfigureEncodingNameOption();
        }

        // TODO: Delete
        public PrintToConsoleMessageHandler2 CreateMessageHandler2(SebaConsole console)
        {
            return new (_outputFormat, _getEncodingName(), console);
        }
        
        public PrintToConsoleMessageHandler CreateMessageHandler(SebaConsole console)
        {
            return new (_outputFormat, _getEncodingName(), console);
        }
    }
}