using System;

namespace ServiceBusAdmin.CommandHandlers.Subscription.Props
{
    public class SubscriptionProps
    {
        public SubscriptionProps(long activeMessageCount, long deadLetterMessageCount)
        {
            ActiveMessageCount = activeMessageCount;
            DeadLetterMessageCount = deadLetterMessageCount;
        }

        public long ActiveMessageCount { get; }
        public long DeadLetterMessageCount { get; }
    }
}