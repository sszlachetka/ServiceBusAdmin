using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace ServiceBusAdmin.CommandHandlers.SendBatch
{
    public class ReadMessagesHandler : IRequestHandler<ReadMessages, IAsyncEnumerable<SendMessageModel>>
    {
        public Task<IAsyncEnumerable<SendMessageModel>> Handle(ReadMessages request, CancellationToken cancellationToken)
        {
            var (filePath, encoding) = request;
            var fileStream = File.OpenRead(filePath);
            IAsyncEnumerable<SendMessageModel> enumerable = new MessageEnumerable(fileStream, encoding);

            return Task.FromResult(enumerable);
        }
    }
}