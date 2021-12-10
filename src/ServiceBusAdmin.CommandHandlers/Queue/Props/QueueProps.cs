namespace ServiceBusAdmin.CommandHandlers.Queue.Props
{
    public record QueueProps(long ActiveMessageCount, long DeadLetterMessageCount);
}