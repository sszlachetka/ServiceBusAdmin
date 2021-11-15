namespace ServiceBusAdmin.Client
{
    public record ReceiverOptions
    {
        public ReceiverOptions(ReceiverEntityName entityName, int maxMessages, bool isDeadLetterSubQueue)
        {
            EntityName = entityName;
            MaxMessages = maxMessages;
            IsDeadLetterSubQueue = isDeadLetterSubQueue;
        }

        public ReceiverEntityName EntityName { get; }
        public int MaxMessages { get; }
        public bool IsDeadLetterSubQueue { get; }
    }
}