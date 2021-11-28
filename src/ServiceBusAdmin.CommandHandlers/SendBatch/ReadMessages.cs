using System.Collections.Generic;
using System.Text;
using MediatR;

namespace ServiceBusAdmin.CommandHandlers.SendBatch
{
    public record ReadMessages(string FilePath, Encoding Encoding) : IRequest<IAsyncEnumerable<SendMessageModel>>;
}