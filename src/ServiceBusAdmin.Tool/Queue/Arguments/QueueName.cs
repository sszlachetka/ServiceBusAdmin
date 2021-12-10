using System;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Queue.Arguments
{
    public static class QueueName
    {
        public static Func<string> ConfigureQueueNameArgument(this CommandLineApplication command)
        {
            var argument = command
                .Argument<string>("Queue name", "")
                .IsRequired();

            return () => argument.ParsedValue;
        }
    }
}