using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using ServiceBusAdmin.Client;

namespace ServiceBusAdmin.Tool.Tests.Subscription
{
    internal static class ServiceBusClientMockExtensions
    {
        public static void SetupReceive(this Mock<IServiceBusClient> mock, ReceiverOptions2 options,
            Func<ReceivedMessageHandler2, Task> handlerCallback)
        {
            mock.Setup(x => x.Receive(options, It.IsAny<ReceivedMessageHandler2>()))
                .Callback(async (ReceiverOptions2 _, ReceivedMessageHandler2 handler) =>
                {
                    await handlerCallback(handler);
                })
                .Returns(Task.CompletedTask);
        }
        
        public static void SetupReceive(this Mock<IServiceBusClient> mock, ReceiverOptions2 options,
            IEnumerable<IReceivedMessage2> messages)
        {
            mock.Setup(x => x.Receive(options, It.IsAny<ReceivedMessageHandler2>()))
                .Callback(async (ReceiverOptions2 _, ReceivedMessageHandler2 handler) =>
                {
                    foreach (var message in messages)
                    {
                        await handler(message);
                    }
                })
                .Returns(Task.CompletedTask);
        }

        public static void SetupSendAnyBinaryDataMessage(this Mock<IServiceBusClient> mock)
        {
            mock.Setup(x => x.SendMessage(It.IsAny<string>(), It.IsAny<BinaryData>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
        }
    }
}