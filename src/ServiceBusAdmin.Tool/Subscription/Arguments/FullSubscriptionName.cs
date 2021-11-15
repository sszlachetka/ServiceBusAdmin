using System;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool.Subscription.Arguments
{
    public static class FullSubscriptionName
    {
        private const string ExpectedFormat = "<topic name>/<subscription name>";
        private const string Name = "Full subscription name";
        private const string Description = "The name must be provided in following format " + ExpectedFormat + ".";

        public static Func<(string topic, string subscription)> ConfigureFullSubscriptionNameArgument(
            this CommandLineApplication command)
        {
            var argument = command
                .Argument<string>(Name, Description)
                .IsRequired();

            return () => ParseFullSubscriptionName(argument);
        }

        private static (string topic, string subscription) ParseFullSubscriptionName(this CommandArgument<string> argument)
        {
            var value = Validate(argument);
            var topicSubscription = value.Split('/');

            return (topicSubscription[0], topicSubscription[1]);
        }

        private static string Validate(CommandArgument<string> arg)
        {
            if (arg.ParsedValue.Count(x => x == '/') != 1)
            {
                throw Failure();
            }

            var topicSubscription = arg.ParsedValue.Split('/');
            if (string.IsNullOrWhiteSpace(topicSubscription[0]) || string.IsNullOrWhiteSpace(topicSubscription[1]))
            {
                throw Failure();
            }

            return arg.ParsedValue;
        }
        
        private static SebaCommandParsingException Failure()
        {
            return new ($"{Name} must be provided in following format {ExpectedFormat}");
        }
    }
}