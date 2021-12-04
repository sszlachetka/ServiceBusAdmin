using System.Threading.Tasks;

namespace ServiceBusAdmin.CommandHandlers.Models
{
    public delegate Task ReceivedMessageCallback(IReceivedMessage message);
}