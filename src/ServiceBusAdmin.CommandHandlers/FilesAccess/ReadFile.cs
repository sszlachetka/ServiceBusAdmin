using System.IO;
using MediatR;

namespace ServiceBusAdmin.CommandHandlers.FilesAccess
{
    public record ReadFile(string FilePath) : IRequest<Stream>;
}