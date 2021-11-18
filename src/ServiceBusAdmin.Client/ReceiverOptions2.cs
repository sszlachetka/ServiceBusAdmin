namespace ServiceBusAdmin.Client
{
    public record ReceiverOptions2
    {
        public ReceiverOptions2(ReceiverEntityName2 entityName, int maxMessages, bool isDeadLetterSubQueue, int messageHandlingConcurrencyLevel)
        {
            EntityName = entityName;
            MaxMessages = maxMessages;
            IsDeadLetterSubQueue = isDeadLetterSubQueue;
            MessageHandlingConcurrencyLevel = messageHandlingConcurrencyLevel;
        }

        public ReceiverEntityName2 EntityName { get; }
        public int MaxMessages { get; }
        public bool IsDeadLetterSubQueue { get; }
        public int MessageHandlingConcurrencyLevel { get; }
    }
}