using System;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Subscription.Arguments
{
    public static class FullSubscriptionName
    {
        private const string Name = "Full subscription name";
        private const string Description = "must be provided in following format <topic name>/<subscription name>";

        public static CommandArgument<string> ConfigureFullSubscriptionNameArgument(this CommandLineApplication command)
        {
            return command
                .Argument<string>(Name, Description)
                .IsRequired();
        }

        public static (string topic, string subscription) ParseFullSubscriptionName(this CommandArgument<string>? argument)
        {
            if (argument == null) throw new ArgumentNullException(nameof(argument));

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
            return new ($"{Name} {Description}");
        }
    }
}