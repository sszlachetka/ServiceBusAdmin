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
    }
}