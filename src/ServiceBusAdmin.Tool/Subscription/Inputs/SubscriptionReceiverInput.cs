using System;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.Client;
using ServiceBusAdmin.Tool.Subscription.Arguments;
using ServiceBusAdmin.Tool.Subscription.Options;

namespace ServiceBusAdmin.Tool.Subscription.Inputs
{
    public class SubscriptionReceiverInput
    {
        private readonly Func<(string topic, string subscription)> _getFullSubscriptionName;
        private readonly Func<int> _getMaxMessages;
        private readonly Func<bool> _getIsDeadLetterSubQueue;

        public SubscriptionReceiverInput(CommandLineApplication command, bool enableDeadLetterSwitch = true)
        {
            _getFullSubscriptionName = command.ConfigureFullSubscriptionNameArgument();
            _getMaxMessages = command.ConfigureMaxMessagesOption();
            _getIsDeadLetterSubQueue = enableDeadLetterSwitch
                ? command.ConfigureIsDeadLetterSubQueue()
                : () => false;
        }
        
        public ReceiverOptions CreateReceiverOptions()
        {
            var (topic, subscription) = _getFullSubscriptionName();

            return new ReceiverOptions(
                new ReceiverEntityName(topic, subscription),
                _getMaxMessages(),
                _getIsDeadLetterSubQueue());
        }
    }
}