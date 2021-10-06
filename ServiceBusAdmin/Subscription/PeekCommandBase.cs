using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Subscription
{
    public abstract class PeekCommandBase : SubscriptionCommandBase
    {
        [Option("-o|--output-format",
            Description = "Provide index-based string format where: 0 - body, 1 - sequence number, 2 - Id, 3 - application properties")]
        public string OutputFormat { get; set; } = "{0}";

        [Option("-e|--encoding",
            Description =
                "Name of encoding used to encode message body. Supported values https://docs.microsoft.com/en-us/dotnet/api/system.text.encoding?view=net-5.0")]
        public string EncodingName { get; set; } = "utf-8";

        [Option("-t|--top", Description = "Count of messages to peek")]
        public int Top { get; set; } = 10;

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            var (topic, subscription) = ParseFullSubscriptionName();
            var options = new ServiceBusReceiverOptions();
            ConfigureOptions(options);
            options.ReceiveMode = ServiceBusReceiveMode.PeekLock;

            await using var client = ServiceBusClient(app);
            await using var receiver = client.CreateReceiver(topic, subscription, options);

            var encoding = Encoding.GetEncoding(EncodingName);
            var peeked = 0;
            ServiceBusReceivedMessage message;
            while (peeked < Top && (message = await receiver.PeekMessageAsync()) != null)
            {
                Console.WriteLine(OutputFormat, 
                    encoding.GetString(message.Body),
                    message.SequenceNumber,
                    message.MessageId,
                    JsonSerializer.Serialize(message.ApplicationProperties));

                peeked++;
            }

            return 0;
        }

        protected virtual void ConfigureOptions(ServiceBusReceiverOptions options)
        {
        }
    }
}