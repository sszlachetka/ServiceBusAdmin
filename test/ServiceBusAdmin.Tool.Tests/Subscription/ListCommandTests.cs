using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests.Subscription
{
    public class ListCommandTests: SebaCommandTests
    {
        [Fact]
        public async Task Lists_all_subscriptions()
        {
            Client.Setup(x => x.GetSubscriptionsNames("topic69", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[] {"sub1", "sub2", "sub3", "sub4"});

            var result = await Seba().Execute(new[] {"subscription", "list", "topic69"});

            AssertSuccess(result);
            AssertConsoleOutput("sub1", "sub2", "sub3", "sub4");
        }
        
        [Fact]
        public async Task Topic_name_argument_is_required()
        {
            var result = await Seba().Execute(new[] {"subscription", "list"});

            AssertFailure(result, "The Topic name field is required.");
        }
    }
}