using System;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Arguments
{
    public static class TopicName
    {
        public static Func<string> ConfigureTopicNameArgument(
            this CommandLineApplication command, string description = "Name of a topic")
        {
            var argument = command
                .Argument<string>("Topic name", description)
                .IsRequired();

            return () => argument.ParsedValue;
        }
    }
}