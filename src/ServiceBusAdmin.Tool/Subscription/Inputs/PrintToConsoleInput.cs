using System;
using System.Text;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.CommandHandlers.Models;
using ServiceBusAdmin.Tool.Options;
using ServiceBusAdmin.Tool.Subscription.Options;

namespace ServiceBusAdmin.Tool.Subscription.Inputs
{
    internal class PrintToConsoleInput
    {
        private readonly Func<MessageBodyFormatEnum> _messageBodyFormat;
        private readonly Func<OutputContentEnum> _outputContent;
        private readonly Func<Encoding> _getEncoding;

        public PrintToConsoleInput(CommandLineApplication command)
        {
            _outputContent = command.ConfigureOutputContentOption();
            _getEncoding = command.ConfigureEncodingNameOption("Name of encoding used to encode message body.");
            _messageBodyFormat = command.ConfigureMessageBodyFormatOption();
        }

        public PrintToConsoleMessageHandler CreateMessageHandler(SebaConsole console)
        {
            return new(_messageBodyFormat(), _outputContent(), _getEncoding(), console);
        }
    }
}