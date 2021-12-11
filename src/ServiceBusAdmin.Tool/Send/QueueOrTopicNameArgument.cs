using System;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Send
{
    public static class QueueOrTopicNameArgument
    {
        public static Func<string> ConfigureQueueOrTopicNameArgument(
            this CommandLineApplication command)
        {
            var argument = command
                .Argument<string>("Queue or topic name", string.Empty)
                .IsRequired();

            return () => argument.ParsedValue;
        }
    }
}