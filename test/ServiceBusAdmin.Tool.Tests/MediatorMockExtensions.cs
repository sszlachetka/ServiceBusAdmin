using System.Threading;
using MediatR;
using Moq;

namespace ServiceBusAdmin.Tool.Tests
{
    internal static class MediatorMockExtensions
    {
        public static void Setup<TRequest, TResponse>(this Mock<IMediator> mock, TResponse response)
            where TRequest : IRequest<TResponse>
        {
            mock.Setup(x => x.Send(It.IsAny<TRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
        }

        public static void Setup<TRequest>(this Mock<IMediator> mock)
            where TRequest : IRequest
        {
            mock.Setup(x => x.Send(It.IsAny<TRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);
        }

        public static void VerifyOnce<TRequest>(this Mock<IMediator> mock, TRequest request)
            where TRequest : IRequest
        {
            mock.Verify(x => x.Send(request, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}