using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace ServiceBusAdmin.Client
{
    public interface IReceivedMessage : IMessage
    {
        Task Complete();
    }

    public class ReceivedMessageAdapter : MessageAdapter, IReceivedMessage
    {
        private readonly ServiceBusReceiver _receiver;

        public ReceivedMessageAdapter(ServiceBusReceivedMessage message, ServiceBusReceiver receiver) : base(message)
        {
            _receiver = receiver;
        }

        public Task Complete()
        {
            return _receiver.CompleteMessageAsync(Message);
        }
    }
}