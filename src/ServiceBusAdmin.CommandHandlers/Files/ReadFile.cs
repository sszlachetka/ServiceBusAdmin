using System.Text;
using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Files
{
    public record ReadFile(string FilePath, Encoding Encoding) : IRequest<string>;
}