using System;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.CommandHandlers;
using ServiceBusAdmin.Tool.Arguments;
using ServiceBusAdmin.Tool.Subscription.Options;

namespace ServiceBusAdmin.Tool.Input
{
    public class ReceiverInput
    {
        private readonly Func<ReceiverEntityName> _getReceiverEntityName;
        private readonly Func<int> _getMaxMessages;
        private readonly Func<bool> _getIsDeadLetterSubQueue;
        private readonly Func<int> _getMessageHandlingConcurrencyLevel;

        public ReceiverInput(
            CommandLineApplication command,
            string? maxMessagesDescription = null, 
            bool enableDeadLetterSwitch = true)
        {
            _getReceiverEntityName = command.ConfigureReceiverEntityNameArgument();
            _getMaxMessages = command.ConfigureMaxMessagesOption(description: maxMessagesDescription);
            _getIsDeadLetterSubQueue = enableDeadLetterSwitch
                ? command.ConfigureIsDeadLetterSubQueue()
                : () => false;
            _getMessageHandlingConcurrencyLevel = command.ConfigureMessageHandlingConcurrencyLevel();
        }

        public ReceiverOptions CreateReceiverOptions()
        {
            return new ReceiverOptions(
                _getReceiverEntityName(),
                _getMaxMessages(),
                _getIsDeadLetterSubQueue(),
                _getMessageHandlingConcurrencyLevel());
        }
    }
}