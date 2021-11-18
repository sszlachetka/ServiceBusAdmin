using System.Threading.Tasks;

namespace ServiceBusAdmin.CommandHandlers
{
    public delegate Task ReceivedMessageHandler(IReceivedMessage message);
}