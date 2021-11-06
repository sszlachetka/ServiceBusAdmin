using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Subscription
{
    // public class DlqResubmitCommand : SubscriptionCommandBase
    // {
    //     [Option("-t|--top", Description = "Count of messages to peek")]
    //     public int Top { get; set; } = 10;
    //     
    //     protected override async Task<int> OnExecute(CommandLineApplication app)
    //     {
    //         var (topic, subscription) = ParseFullSubscriptionName();
    //
    //         await using var client = ServiceBusClient(app);
    //         await using var sender = client.CreateSender(topic);
    //         await using var receiver = client.CreateReceiver(topic, subscription,
    //             new ServiceBusReceiverOptions
    //             {
    //                 ReceiveMode = ServiceBusReceiveMode.PeekLock,
    //                 SubQueue = SubQueue.DeadLetter
    //             });
    //
    //         var received = 0;
    //         ServiceBusReceivedMessage receivedMessage;
    //         while (received < Top && (receivedMessage = await receiver.ReceiveMessageAsync()) != null)
    //         {
    //             received++;
    //
    //             var message = new ServiceBusMessage(receivedMessage);
    //             await sender.SendMessageAsync(message);
    //             await receiver.CompleteMessageAsync(receivedMessage);
    //
    //             Console.WriteLine(receivedMessage.MessageId);
    //         }
    //
    //         return 0;
    //     }
    // }
}