using System;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Options
{
    public static class IsDeadLetterSubQueue
    {
        public const string Template = "-dlq|--dead-letter-queue";

        public static Func<bool> ConfigureIsDeadLetterSubQueue(this CommandLineApplication command)
        {
            var option = command.Option(
                Template,
                "Fetch messages from dead letter sub-queue, which corresponds to specified service bus entity.",
                CommandOptionType.NoValue);

            return () => option.HasValue();
        }
    }
}