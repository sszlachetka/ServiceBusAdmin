using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Subscription
{
    [Command("peek-dlq")]
    public class PeekDlqCommand : SubscriptionCommandBase
    {
        [Option("-o|--output-format", Description = "Provide index-based string format where: 0 - body, 1 - sequence number, 2 - Id")]
        public string OutputFormat { get; set; } = "{0}";

        [Option("-e|--encoding", Description = "Name of encoding used to encode message body. Supported values https://docs.microsoft.com/en-us/dotnet/api/system.text.encoding?view=net-5.0")]
        public string EncodingName { get; set; } = "utf-8";

        [Option("-t|--top", Description = "Count of messages to peek")]
        public int Top { get; set; } = 10;
        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            var (topic, subscription) = ParseFullSubscriptionName();

            await using var client = ServiceBusClient(app);
            await using var receiver = client.CreateReceiver(topic, subscription,
                new ServiceBusReceiverOptions
                {
                    ReceiveMode = ServiceBusReceiveMode.PeekLock,
                    SubQueue = SubQueue.DeadLetter
                });

            var peeked = 0;
            ServiceBusReceivedMessage message;
            while (peeked < Top && (message = await receiver.PeekMessageAsync()) != null)
            {
                var encoding = Encoding.GetEncoding(EncodingName);

                Console.WriteLine(OutputFormat, encoding.GetString(message.Body), message.SequenceNumber,
                    message.MessageId);

                peeked++;
            }

            return 0;
        }
    }
}