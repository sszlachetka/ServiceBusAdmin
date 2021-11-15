using System.Threading.Tasks;

namespace ServiceBusAdmin.Client
{
    public delegate Task ReceivedMessageHandler(IReceivedMessage message);
}