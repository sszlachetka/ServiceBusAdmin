using ServiceBusAdmin.Client;

namespace ServiceBusAdmin.Tool.Tests.Subscription
{
    public class ReceiverOptionsBuilder2
    {
        private ReceiverEntityName2 _receiverEntityName = new ("someTopic", "someSubscription");
        private int _maxMessages = 10;
        private bool _isDeadLetterSubQueue;
        private int _messageHandlingConcurrencyLevel = 1;

        public ReceiverOptionsBuilder2 WithEntityName(ReceiverEntityName2 value)
        {
            _receiverEntityName = value;
            return this;
        }
        
        public ReceiverOptionsBuilder2 WithMaxMessages(int value)
        {
            _maxMessages = value;
            return this;
        }
        
        public ReceiverOptionsBuilder2 WithIsDeadLetterSubQueue(bool value)
        {
            _isDeadLetterSubQueue = value;
            return this;
        }
        
        public ReceiverOptionsBuilder2 WithMessageHandlingConcurrencyLevel(int value)
        {
            _messageHandlingConcurrencyLevel = value;
            return this;
        }

        public ReceiverOptions2 Build()
        {
            return new (_receiverEntityName, _maxMessages, _isDeadLetterSubQueue, _messageHandlingConcurrencyLevel);
        }
    }
}