using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace ServiceBusAdmin.CommandHandlers
{
    public interface IReceivedMessage : IMessage
    {
        Task Complete();
        Task DeadLetter();
    }

    public class ReceivedMessageAdapter : PeekedMessageAdapter, IReceivedMessage
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

        public Task DeadLetter()
        {
            return _receiver.DeadLetterMessageAsync(Message);
        }
    }
}