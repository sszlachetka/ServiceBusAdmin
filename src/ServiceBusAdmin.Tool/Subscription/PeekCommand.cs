using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.Client;
using ServiceBusAdmin.Tool.Subscription.Arguments;
using ServiceBusAdmin.Tool.Subscription.Options;

namespace ServiceBusAdmin.Tool.Subscription
{
    public class PeekCommand : SebaCommand
    {
        private readonly Func<(string topic, string subscription)> _getFullSubscriptionName;
        private readonly OutputFormatOption _outputFormat;
        private readonly Func<string> _getEncodingName;
        private readonly Func<int> _getTop;

        public PeekCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            _getFullSubscriptionName = Command.ConfigureFullSubscriptionNameArgument();
            _outputFormat = Command.ConfigureOutputFormatOption();
            _getEncodingName = Command.ConfigureEncodingNameOption();
            _getTop = Command.ConfigureTopOption("Count of messages to peek");
        }

        protected override async Task Execute(CancellationToken cancellationToken)
        {
            var messageHandler = CreateMessageHandler();
            var options = CreateTopicReceiverOptions();

            await Client.Peek(options, messageHandler.Handle);
        }

        private MessageHandler CreateMessageHandler()
        {
            return new (_outputFormat, _getEncodingName(), Console);
        }
        
        private TopicReceiverOptions CreateTopicReceiverOptions()
        {
            var (topic, subscription) = _getFullSubscriptionName();

            return new TopicReceiverOptions(topic, subscription, ServiceBusReceiveMode.PeekLock, _getTop());
        }

        private class MessageHandler
        {
            private readonly OutputFormatOption _outputFormat;
            private readonly Encoding _encoding;
            private readonly SebaConsole _console;

            public MessageHandler(OutputFormatOption outputFormat, string encodingName, SebaConsole console)
            {
                _outputFormat = outputFormat;
                _encoding = Encoding.GetEncoding(encodingName);
                _console = console;
            }

            public Task Handle(IServiceBusMessage message)
            {
                _console.Info(_outputFormat.Value,
                    _outputFormat.IncludesMessageBody ? _encoding.GetString(message.Body) : string.Empty,
                    message.SequenceNumber,
                    message.MessageId,
                    _outputFormat.IncludesApplicationProperties
                        ? JsonSerializer.Serialize(message.ApplicationProperties)
                        : string.Empty);

                return Task.CompletedTask;
            }
        }
    }
}