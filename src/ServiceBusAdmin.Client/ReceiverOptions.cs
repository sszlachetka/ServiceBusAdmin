namespace ServiceBusAdmin.Client
{
    public record ReceiverOptions
    {
        public ReceiverOptions(string topicName, string subscriptionName, int maxMessages)
        {
            TopicName = topicName;
            SubscriptionName = subscriptionName;
            MaxMessages = maxMessages;
        }

        public string TopicName { get; }
        public string SubscriptionName { get; }
        public int MaxMessages { get; }
    }
}