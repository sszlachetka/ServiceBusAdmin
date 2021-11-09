using System.Threading.Tasks;

namespace ServiceBusAdmin.Client
{
    public delegate Task ServiceBusMessageHandler(IServiceBusMessage message);
}