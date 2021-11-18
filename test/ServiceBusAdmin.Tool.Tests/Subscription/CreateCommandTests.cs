using System.Threading.Tasks;
using ServiceBusAdmin.CommandHandlers.Subscription.Create;
using ServiceBusAdmin.Tool.Tests.TestData;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests.Subscription
{
    public class CreateCommandTests : SebaCommandTests
    {
        [Fact]
        public async Task Creates_subscription_with_provided_name()
        {
            Mediator.Setup<CreateSubscription>();

            var result = await Seba().Execute(new[] {"subscription", "create", "topic69/subscription1"});

            AssertSuccess(result);
            Mediator.VerifyOnce(new CreateSubscription("topic69", "subscription1"));
        }

        [Fact]
        public async Task Full_subscription_name_argument_is_required()
        {
            var result = await Seba().Execute(new[] {"subscription", "create"});

            AssertFailure(result, "The Full subscription name field is required.");
        }
        
        [Theory]
        [ClassData(typeof(InvalidFullSubscriptionName))]
        public async Task Invalid_full_subscription_name_format_is_rejected(string invalidFullSubscriptionName)
        {
            var result = await Seba().Execute(new[] {"subscription", "create", invalidFullSubscriptionName});

            AssertFailure(result, "Full subscription name must be provided in following format <topic name>/<subscription name>");
        }
    }
}