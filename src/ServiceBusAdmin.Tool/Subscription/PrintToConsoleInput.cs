using System;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.Client;
using ServiceBusAdmin.Tool.Subscription.Arguments;
using ServiceBusAdmin.Tool.Subscription.Options;

namespace ServiceBusAdmin.Tool.Subscription
{
    internal class PrintToConsoleInput
    {
        private readonly Func<(string topic, string subscription)> _getFullSubscriptionName;
        private readonly OutputFormatOption _outputFormat;
        private readonly Func<string> _getEncodingName;
        private readonly Func<int> _getMaxMessages;

        public PrintToConsoleInput(CommandLineApplication command)
        {
            _getFullSubscriptionName = command.ConfigureFullSubscriptionNameArgument();
            _outputFormat = command.ConfigureOutputFormatOption();
            _getEncodingName = command.ConfigureEncodingNameOption();
            _getMaxMessages = command.ConfigureMaxMessagesOption();
        }

        public PrintToConsoleMessageHandler CreatePrintToConsoleMessageHandler(SebaConsole console)
        {
            return new (_outputFormat, _getEncodingName(), console);
        }

        public ReceiverOptions CreateTopicReceiverOptions()
        {
            var (topic, subscription) = _getFullSubscriptionName();

            return new ReceiverOptions(new ReceiverEntityName(topic, subscription), _getMaxMessages());
        }
    }
}