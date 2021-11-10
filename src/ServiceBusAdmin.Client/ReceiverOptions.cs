namespace ServiceBusAdmin.Client
{
    public record ReceiverOptions
    {
        public ReceiverOptions(ReceiverEntityName entityName, int maxMessages)
        {
            EntityName = entityName;
            MaxMessages = maxMessages;
        }

        public ReceiverEntityName EntityName { get; }
        public int MaxMessages { get; }
    }
}