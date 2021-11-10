using System;

namespace ServiceBusAdmin.Client
{
    public record ReceiverEntityName
    {
        private readonly string? _topicName;
        private readonly string? _subscriptionName;
        private readonly string? _queueName;

        public ReceiverEntityName(string queueName)
        {
            _queueName = queueName;
        }

        public ReceiverEntityName(string topicName, string subscriptionName)
        {
            _topicName = topicName;
            _subscriptionName = subscriptionName;
        }

        public string QueueName() => _queueName ?? throw new InvalidOperationException("Queue name is undefined");

        public string TopicName() => _topicName ?? throw new InvalidOperationException("Topic name is undefined");

        public string SubscriptionName() =>
            _subscriptionName ?? throw new InvalidOperationException("Subscription name is undefined");

        public bool IsQueue => _queueName != null;

        public override string ToString()
        {
            return IsQueue ? _queueName! : string.Concat(_topicName, "/", _subscriptionName);
        }
    }
}