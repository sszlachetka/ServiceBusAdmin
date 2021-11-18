using System.Threading.Tasks;

namespace ServiceBusAdmin.Client
{
    public delegate Task ReceivedMessageHandler2(IReceivedMessage2 message);
}