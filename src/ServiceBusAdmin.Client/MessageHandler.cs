using System.Threading.Tasks;

namespace ServiceBusAdmin.Client
{
    public delegate Task MessageHandler(IMessage message);
}