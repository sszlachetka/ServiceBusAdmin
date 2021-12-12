using System;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.CommandHandlers;
using ServiceBusAdmin.Tool.Subscription.Arguments;

namespace ServiceBusAdmin.Tool.Arguments
{
    public static class ReceiverEntityNameArgument
    {
        private const string Name = "Queue or full subscription name";
        private const string Description = "Full subscription name must be provided in following format " + FullSubscriptionName.ExpectedFormat + ".";

        public static Func<ReceiverEntityName> ConfigureReceiverEntityNameArgument(
            this CommandLineApplication command)
        {
            var argument = command
                .Argument<string>(Name, Description)
                .IsRequired();

            return () => ParseQueueOrSubscriptionName(argument);
        }

        private static ReceiverEntityName ParseQueueOrSubscriptionName(this CommandArgument<string> argument)
        {
            var parsedValue = argument.ParsedValue;
            if (!parsedValue.Contains('/')) return new ReceiverEntityName(parsedValue);

            var (topic, subscription) = argument.ParseFullSubscriptionName();
            
            return new ReceiverEntityName(topic, subscription);
        }
    }
}