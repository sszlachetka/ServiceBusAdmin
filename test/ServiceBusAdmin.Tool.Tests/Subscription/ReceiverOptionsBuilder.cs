using ServiceBusAdmin.Client;

namespace ServiceBusAdmin.Tool.Tests.Subscription
{
    public class ReceiverOptionsBuilder
    {
        private ReceiverEntityName _receiverEntityName = new ("someTopic", "someSubscription");
        private int _maxMessages = 10;
        private bool _isDeadLetterSubQueue;
        private int _messageHandlingConcurrencyLevel = 1;

        public ReceiverOptionsBuilder WithEntityName(ReceiverEntityName value)
        {
            _receiverEntityName = value;
            return this;
        }
        
        public ReceiverOptionsBuilder WithMaxMessages(int value)
        {
            _maxMessages = value;
            return this;
        }
        
        public ReceiverOptionsBuilder WithIsDeadLetterSubQueue(bool value)
        {
            _isDeadLetterSubQueue = value;
            return this;
        }
        
        public ReceiverOptionsBuilder WithMessageHandlingConcurrencyLevel(int value)
        {
            _messageHandlingConcurrencyLevel = value;
            return this;
        }

        public ReceiverOptions Build()
        {
            return new (_receiverEntityName, _maxMessages, _isDeadLetterSubQueue, _messageHandlingConcurrencyLevel);
        }
    }
}