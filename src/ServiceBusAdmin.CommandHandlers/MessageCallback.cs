using System.Threading.Tasks;

namespace ServiceBusAdmin.CommandHandlers
{
    public delegate Task MessageCallback(IMessage message);
}