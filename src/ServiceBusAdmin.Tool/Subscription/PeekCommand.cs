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
        private readonly Func<string> _getOutputFormat;
        private readonly Func<string> _getEncodingName;
        private readonly Func<int> _getTop;

        public PeekCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            _getFullSubscriptionName = Command.ConfigureFullSubscriptionNameArgument();
            _getOutputFormat = Command.ConfigureOutputFormatOption();
            _getEncodingName = Command.ConfigureEncodingNameOption();
            _getTop = Command.ConfigureTopOption("Count of messages to peek");
        }

        protected override async Task<SebaResult> Execute(CancellationToken cancellationToken)
        {
            var messageHandler = CreateMessageHandler();
            var options = CreateTopicReceiverOptions();
            var client = CreateClient();

            await client.Peek(options, messageHandler.Handle);

            return SebaResult.Success;
        }

        private MessageHandler CreateMessageHandler()
        {
            return new (_getOutputFormat(), _getEncodingName(), Output);
        }
        
        private TopicReceiverOptions CreateTopicReceiverOptions()
        {
            var (topic, subscription) = _getFullSubscriptionName();

            return new TopicReceiverOptions(topic, subscription, ServiceBusReceiveMode.PeekLock, _getTop());
        }

        private class MessageHandler
        {
            private readonly string _outputFormat;
            private readonly Encoding _encoding;
            private readonly IOutputWriter _outputWriter;

            public MessageHandler(string outputFormat, string encodingName, IOutputWriter outputWriter)
            {
                _outputFormat = outputFormat;
                _encoding = Encoding.GetEncoding(encodingName);
                _outputWriter = outputWriter;
            }

            public Task Handle(ServiceBusReceivedMessage message)
            {
                _outputWriter.WriteLine(_outputFormat, 
                    _encoding.GetString(message.Body),
                    message.SequenceNumber,
                    message.MessageId,
                    JsonSerializer.Serialize(message.ApplicationProperties));

                return Task.CompletedTask;
            }
        }
    }
}