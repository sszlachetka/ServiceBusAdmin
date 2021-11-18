using System.Threading.Tasks;

namespace ServiceBusAdmin.CommandHandlers
{
    public delegate Task MessageHandler(IMessage message);
}