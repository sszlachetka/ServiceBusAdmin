namespace ServiceBusAdmin.CommandHandlers.Status
{
    public record EntityProperties(
        EntityType EntityType,
        string EntityName,
        long ActiveMessageCount,
        long DeadLetterMessageCount);
}