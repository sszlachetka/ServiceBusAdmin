using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace ServiceBusAdmin.Client
{
    public interface IReceivedMessage2 : IMessage2
    {
        Task Complete();
        Task DeadLetter();
    }

    public class ReceivedMessage2Adapter : Message2Adapter, IReceivedMessage2
    {
        private readonly ServiceBusReceiver _receiver;

        public ReceivedMessage2Adapter(ServiceBusReceivedMessage message, ServiceBusReceiver receiver) : base(message)
        {
            _receiver = receiver;
        }

        public Task Complete()
        {
            return _receiver.CompleteMessageAsync(Message);
        }

        public Task DeadLetter()
        {
            return _receiver.DeadLetterMessageAsync(Message);
        }
    }
}