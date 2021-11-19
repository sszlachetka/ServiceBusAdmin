using System;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.Tool.Subscription.Options;

namespace ServiceBusAdmin.Tool.Subscription.Inputs
{
    internal class PrintToConsoleInput
    {
        private readonly Func<OutputContentEnum> _outputContent;
        private readonly Func<string> _getEncodingName;

        public PrintToConsoleInput(CommandLineApplication command)
        {
            _outputContent = command.ConfigureOutputContentOption();
            _getEncodingName = command.ConfigureEncodingNameOption();
        }

        public PrintToConsoleMessageHandler CreateMessageHandler(SebaConsole console)
        {
            return new (_outputContent(), _getEncodingName(), console);
        }
    }
}