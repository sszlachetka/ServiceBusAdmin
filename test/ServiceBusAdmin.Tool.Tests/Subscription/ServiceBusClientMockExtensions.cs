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
        public static void SetupPeek(this Mock<IServiceBusClient> mock, ReceiverOptions options,
            Func<MessageHandler, Task> handlerCallback)
        {
            mock.Setup(x => x.Peek(options, It.IsAny<MessageHandler>()))
                .Callback(async (ReceiverOptions _, MessageHandler handler) =>
                {
                    await handlerCallback(handler);
                })
                .Returns(Task.CompletedTask);
        }

        public static void SetupReceive(this Mock<IServiceBusClient> mock, ReceiverOptions options,
            Func<ReceivedMessageHandler, Task> handlerCallback)
        {
            mock.Setup(x => x.Receive(options, It.IsAny<ReceivedMessageHandler>()))
                .Callback(async (ReceiverOptions _, ReceivedMessageHandler handler) =>
                {
                    await handlerCallback(handler);
                })
                .Returns(Task.CompletedTask);
        }
        
        public static void SetupReceive(this Mock<IServiceBusClient> mock, ReceiverOptions options,
            IEnumerable<IReceivedMessage> messages)
        {
            mock.Setup(x => x.Receive(options, It.IsAny<ReceivedMessageHandler>()))
                .Callback(async (ReceiverOptions _, ReceivedMessageHandler handler) =>
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