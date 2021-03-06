using System;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Queue
{
    public static class QueueNameArgument
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