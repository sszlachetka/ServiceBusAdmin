using System.Threading;
using System.Threading.Tasks;
using Moq;
using ServiceBusAdmin.CommandHandlers.Subscription.Delete;
using ServiceBusAdmin.Tool.Tests.TestData;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests.Subscription
{
    public class DeleteCommandTests : SebaCommandTests
    {
        [Fact]
        public async Task Deletes_subscription_with_provided_name()
        {
            Mediator.Setup<DeleteSubscription>();

            var result = await Seba().Execute(new[] {"subscription", "delete", "topic69/subscription1"});

            AssertSuccess(result);
            Mediator.VerifyOnce(new DeleteSubscription("topic69", "subscription1"));
        }

        [Fact]
        public async Task Full_subscription_name_argument_is_required()
        {
            var result = await Seba().Execute(new[] {"subscription", "delete"});

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