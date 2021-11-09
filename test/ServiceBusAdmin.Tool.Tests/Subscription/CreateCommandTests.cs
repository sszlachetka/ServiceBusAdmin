using System.Threading;
using System.Threading.Tasks;
using Moq;
using ServiceBusAdmin.Tool.Tests.TestData;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests.Subscription
{
    public class CreateCommandTests : SebaCommandTests
    {
        public CreateCommandTests()
        {
            Client.Setup(x =>
                    x.CreateSubscription(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
        }

        [Fact]
        public async Task Creates_subscription_with_provided_name()
        {
            var result = await Seba().Execute(new[] {"subscription", "create", "topic69/subscription1"});

            AssertSuccess(result);
            Client.Verify(x => x.CreateSubscription("topic69", "subscription1", It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Full_subscription_name_argument_is_required()
        {
            var result = await Seba().Execute(new[] {"subscription", "create"});

            AssertFailure(result, "The Full subscription name field is required.");
            Client.Verify(
                x => x.CreateSubscription(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }
        
        [Theory]
        [ClassData(typeof(InvalidFullSubscriptionName))]
        public async Task Invalid_full_subscription_name_format_is_rejected(string invalidFullSubscriptionName)
        {
            var result = await Seba().Execute(new[] {"subscription", "create", invalidFullSubscriptionName});

            AssertFailure(result, "Full subscription name must be provided in following format <topic name>/<subscription name>");
            Client.Verify(
                x => x.CreateSubscription(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}