using System.Threading.Tasks;

namespace ServiceBusAdmin.CommandHandlers.Models
{
    public delegate Task MessageCallback(IMessage message);
}