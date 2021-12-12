using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using MediatR;
using Moq;
using ServiceBusAdmin.CommandHandlers;
using ServiceBusAdmin.CommandHandlers.Models;
using ServiceBusAdmin.CommandHandlers.Receive;
using ServiceBusAdmin.CommandHandlers.Send;

namespace ServiceBusAdmin.Tool.Tests.Receive
{
    internal static class ReceiveMessagesMediatorMockExtensions
    {
        public static IReadOnlyCollection<Exception> SetupReceiveMessages(this Mock<IMediator> mock, ReceiverOptions options,
            params IReceivedMessage[] receivedMessages)
        {
            var exceptions = new List<Exception>();
            mock.Setup<ReceiveMessages>(
                request => request.Options == options,
                async receiveMessages =>
                {
                    try
                    {
                        foreach (var message in receivedMessages)
                        {
                            await receiveMessages.Callback(message);
                        }
                    }
                    catch (Exception e)
                    {
                        // Exceptions thrown by callback supplied to mock's setup
                        // are swallowed by Moq but some of the tests must assert
                        // them. Setup callback is calling ReceivedMessageCallback
                        // on each message, which executes code that is being tested.
                        exceptions.Add(e);
                    }
                });

            return exceptions;
        }

        public static void VerifyReceiveMessagesOnce(this Mock<IMediator> mock, ReceiverOptions options)
        {
            mock.Verify(x => x.Send(
                    It.Is<ReceiveMessages>(request => request.Options == options),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        public static void SetupSendAnyMessage(this Mock<IMediator> mock)
        {
            mock.Setup(x => x.Send(It.IsAny<SendMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);
        }

        public static void VerifySendMessageOnce(this Mock<IMediator> mock, string queueOrTopicName,
            BinaryData messageBody)
        {
            Expression<Func<SendMessage, bool>> match = command =>
                command.QueueOrTopicName == queueOrTopicName && ReferenceEquals(command.Message.Body, messageBody);

            mock.Verify(x => x.Send(It.Is(match), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}