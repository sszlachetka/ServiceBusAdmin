using System;

namespace ServiceBusAdmin.CommandHandlers.Subscription.Props
{
    public record SubscriptionProps(long ActiveMessageCount, long DeadLetterMessageCount);
}