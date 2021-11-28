using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using MediatR;
using Moq;
using ServiceBusAdmin.CommandHandlers;
using ServiceBusAdmin.CommandHandlers.Send;
using ServiceBusAdmin.CommandHandlers.Subscription.Receive;

namespace ServiceBusAdmin.Tool.Tests.Subscription.Receive
{
    internal static class ReceiveMessagesMediatorMockExtensions
    {
        public static void SetupReceiveMessages(this Mock<IMediator> mock, ReceiverOptions options,
            params IReceivedMessage[] receivedMessages)
        {
            mock.Setup<ReceiveMessages>(
                request => request.Options == options,
                async receiveMessages =>
                {
                    foreach (var message in receivedMessages)
                    {
                        await receiveMessages.Callback(message);
                    }
                });
        }

        public static void VerifyReceiveMessagesOnce(this Mock<IMediator> mock, ReceiverOptions options)
        {
            mock.Verify(
                x => x.Send(It.Is<ReceiveMessages>(request => request.Options == options), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        public static void SetupSendAnyBinaryMessage(this Mock<IMediator> mock)
        {
            mock.Setup(x => x.Send(It.IsAny<SendBinaryMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);
        }
        
        public static void VerifySendBinaryMessageOnce(this Mock<IMediator> mock, string topicName, BinaryData messageBody)
        {
            Expression<Func<SendBinaryMessage,bool>> match = message =>
                message.QueueOrTopicName == topicName && ReferenceEquals(message.MessageBody, messageBody);

            mock.Verify(x => x.Send(It.Is(match), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}