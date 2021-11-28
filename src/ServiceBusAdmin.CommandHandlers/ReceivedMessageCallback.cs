using System.Threading.Tasks;

namespace ServiceBusAdmin.CommandHandlers
{
    public delegate Task ReceivedMessageCallback(IReceivedMessage message);
}