using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.Subscription.Arguments;
using ServiceBusAdmin.Subscription.Options;

namespace ServiceBusAdmin.Subscription
{
    public class PeekCommand : SebaCommand
    {
        private CommandArgument<string>? _argument;
        private CommandOption<string?>? _outputFormatOption;
        private CommandOption<string?>? _encodingNameOption;
        private CommandOption<int?>? _topOption;

        public PeekCommand(SebaContext context) : base(context)
        {
        }

        protected override void ConfigureArgsAndOptions(CommandLineApplication command)
        {
            _argument = command.ConfigureFullSubscriptionNameArgument();
            _outputFormatOption = command.ConfigureOutputFormatOption();
            _encodingNameOption = command.ConfigureEncodingNameOption();
            _topOption = command.ConfigureTopOption("Count of messages to peek");
        }

        protected override async Task ExecuteAsync(CommandLineApplication command, CancellationToken cancellationToken)
        {
            var messageHandler = CreateMessageHandler();
            var options = CreateTopicReceiverOptions();
            var client = CreateServiceBusClient();

            await client.Peek(options, messageHandler.Handle);
        }

        private MessageHandler CreateMessageHandler()
        {
            var outputFormat = _outputFormatOption.ParseOutputFormat();
            var encodingName = _encodingNameOption.ParseEncodingName();

            return new MessageHandler(outputFormat, encodingName, Output);
        }
        
        private TopicReceiverOptions CreateTopicReceiverOptions()
        {
            var (topic, subscription) = _argument.ParseFullSubscriptionName();
            var top = _topOption.ParseTop();

            return new TopicReceiverOptions(topic, subscription, ServiceBusReceiveMode.PeekLock, top);
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