using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests.Subscription
{
    public class DeleteCommandTests : SebaCommandTests
    {
        public DeleteCommandTests()
        {
            Client.Setup(x =>
                    x.DeleteSubscription(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
        }

        [Fact]
        public async Task Deletes_subscription_with_provided_name()
        {
            var result = await Seba().Execute(new[] {"subscription", "delete", "topic69/subscription1"});

            AssertSuccess(result);
            Client.Verify(x => x.DeleteSubscription("topic69", "subscription1", It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Full_subscription_name_argument_is_mandatory()
        {
            var result = await Seba().Execute(new[] {"subscription", "delete"});

            AssertFailure(result, "The Full subscription name field is required.");
            Client.Verify(
                x => x.DeleteSubscription(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }
        
        [Theory]
        [InlineData("topic")]
        [InlineData("topic/")]
        [InlineData("/subscription")]
        public async Task Invalid_full_subscription_name_format_is_rejected(string invalidFullSubscriptionName)
        {
            var result = await Seba().Execute(new[] {"subscription", "create", invalidFullSubscriptionName});

            AssertFailure(result, "Full subscription name must be provided in following format <topic name>/<subscription name>");
            Client.Verify(
                x => x.DeleteSubscription(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}