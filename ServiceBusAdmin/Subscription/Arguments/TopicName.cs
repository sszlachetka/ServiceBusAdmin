using System;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Subscription.Arguments
{
    public static class TopicName
    {
        public static Func<string> ConfigureTopicNameArgument(
            this CommandLineApplication command)
        {
            var argument = command
                .Argument<string>("Topic name", "Name of a topic")
                .IsRequired();

            return () => argument.ParsedValue;
        }
    }
}