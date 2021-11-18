namespace ServiceBusAdmin.CommandHandlers
{
    public record ReceiverOptions
    {
        public ReceiverOptions(ReceiverEntityName entityName, int maxMessages, bool isDeadLetterSubQueue, int messageHandlingConcurrencyLevel)
        {
            EntityName = entityName;
            MaxMessages = maxMessages;
            IsDeadLetterSubQueue = isDeadLetterSubQueue;
            MessageHandlingConcurrencyLevel = messageHandlingConcurrencyLevel;
        }

        public ReceiverEntityName EntityName { get; }
        public int MaxMessages { get; }
        public bool IsDeadLetterSubQueue { get; }
        public int MessageHandlingConcurrencyLevel { get; }
    }
}