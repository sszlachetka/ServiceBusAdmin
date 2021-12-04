using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace ServiceBusAdmin.CommandHandlers.FilesAccess
{
    internal class ReadFileHandler : IRequestHandler<ReadFile, Stream>
    {
        public Task<Stream> Handle(ReadFile request, CancellationToken cancellationToken)
        {
            return Task.FromResult((Stream)File.OpenRead(request.FilePath));
        }
    }
}