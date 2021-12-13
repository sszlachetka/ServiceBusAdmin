using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
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
        
        public static void Setup<TRequest>(this Mock<IMediator> mock, 
            Expression<Func<TRequest, bool>> requestMatch,
            Func<TRequest, Task> callback)
            where TRequest : IRequest
        {
            mock.Setup(x => x.Send(It.Is(requestMatch), It.IsAny<CancellationToken>()))
                .Callback((IRequest<Unit> request, CancellationToken _) => callback((TRequest)request))
                .ReturnsAsync(Unit.Value);
        }
        
        public static void Setup<TRequest>(this Mock<IMediator> mock, 
            Func<TRequest, Task> callback)
            where TRequest : IRequest
        {
            mock.Setup(x => x.Send(It.IsAny<TRequest>(), It.IsAny<CancellationToken>()))
                .Callback((IRequest<Unit> request, CancellationToken _) => callback((TRequest)request))
                .ReturnsAsync(Unit.Value);
        }

        public static void VerifyOnce<TRequest>(this Mock<IMediator> mock, TRequest request)
            where TRequest : IRequest
        {
            mock.Verify(x => x.Send(request, It.IsAny<CancellationToken>()), Times.Once);
        }
        
        public static void VerifyOnce<TRequest>(this Mock<IMediator> mock, Expression<Func<TRequest, bool>> requestMatch)
            where TRequest : IRequest
        {
            mock.Verify(x => x.Send(It.Is(requestMatch), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}