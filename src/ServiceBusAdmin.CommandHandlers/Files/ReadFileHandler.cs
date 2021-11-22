using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Files
{
    internal class ReadFileHandler : IRequestHandler<ReadFile, string>
    {
        public Task<string> Handle(ReadFile request, CancellationToken cancellationToken)
        {
            var (filePath, encoding) = request;

            return File.ReadAllTextAsync(filePath, encoding, cancellationToken);
        }
    }
}