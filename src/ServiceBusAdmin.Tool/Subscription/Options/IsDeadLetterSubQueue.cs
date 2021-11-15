using System;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Subscription.Options
{
    public static class IsDeadLetterSubQueue
    {
        public static Func<bool> ConfigureIsDeadLetterSubQueue(this CommandLineApplication command)
        {
            var option = command.Option(
                "-dlq|--dead-letter-queue",
                "Fetch messages from dead letter sub-queue, which corresponds to specified service bus entity.",
                CommandOptionType.NoValue);

            return () => option.HasValue();
        }
    }
}