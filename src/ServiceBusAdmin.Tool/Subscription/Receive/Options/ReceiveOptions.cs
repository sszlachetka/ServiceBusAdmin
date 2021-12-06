using System;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Subscription.Receive.Options
{
    public class ReceiveOptions
    {
        private readonly Func<long[]> _getHandleSequenceNumbers;
        private readonly Func<bool> _getAbandonUnhandledMessages;

        public ReceiveOptions(Func<long[]> getHandleSequenceNumbers, Func<bool> getAbandonUnhandledMessages)
        {
            _getHandleSequenceNumbers = getHandleSequenceNumbers;
            _getAbandonUnhandledMessages = getAbandonUnhandledMessages;
        }

        public ReceivedMessageHandlingPolicy CreateMessageHandlingPolicy(SebaConsole console)
        {
            return new ReceivedMessageHandlingPolicy(
                console,
                _getHandleSequenceNumbers(),
                _getAbandonUnhandledMessages());
        }
    }

    public static class ReceiveOptionsConfigurator
    {
        public static ReceiveOptions ConfigureReceiveOptions(this CommandLineApplication command)
        {
            var getHandleSequenceNumbers = command.ConfigureHandleSequenceNumbers();
            var getAbandonUnhandledMessages = command.ConfigureAbandonUnhandledMessages();

            return new ReceiveOptions(getHandleSequenceNumbers, getAbandonUnhandledMessages);
        }
    }
}