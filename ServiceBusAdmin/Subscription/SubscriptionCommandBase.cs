using System.ComponentModel.DataAnnotations;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Subscription
{
    public abstract class SubscriptionCommandBase : SbaCommandBase
    {
        private const string FullSubscriptionArgumentName = "Full subscription name";

        private const string FullSubscriptionArgumentDescription = "Must be in following format <topic name>/<subscription name>";
        
        [Required]
        [ValidFullSubscriptionNameAttribute]
        [Argument(0, FullSubscriptionArgumentName, FullSubscriptionArgumentDescription)]
        protected string FullSubscriptionName { get; set; }

        protected (string topicName, string subscriptionName) ParseFullSubscriptionName()
        {
            var topicSubscription = FullSubscriptionName.Split('/');

            return (topicSubscription[0], topicSubscription[1]);
        }

        private class ValidFullSubscriptionNameAttribute : ValidationAttribute
        {
            public ValidFullSubscriptionNameAttribute()
                : base("{0} " + FullSubscriptionArgumentDescription)
            {
            }

            protected override ValidationResult IsValid(object value, ValidationContext context)
            {
                if (value is not string fullName || fullName.Count(x => x == '/') != 1)
                {
                    return new ValidationResult(FormatErrorMessage(context.DisplayName));
                }

                var topicSubscription = fullName.Split('/');
                if (string.IsNullOrWhiteSpace(topicSubscription[0]) || string.IsNullOrWhiteSpace(topicSubscription[1]))
                {
                    return new ValidationResult(FormatErrorMessage(context.DisplayName));
                }

                return ValidationResult.Success;
            }
        }
    }
}