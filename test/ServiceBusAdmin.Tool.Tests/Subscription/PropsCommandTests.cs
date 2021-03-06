using System.Threading;
using System.Threading.Tasks;
using Moq;
using ServiceBusAdmin.CommandHandlers.Subscription.Props;
using ServiceBusAdmin.Tool.Tests.TestData;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests.Subscription
{
    public class PropsCommandTests: SebaCommandTests
    {
        [Fact]
        public async Task Returns_subscription_properties()
        {
            Mediator.Setup<GetSubscriptionProps, SubscriptionProps>(
                new SubscriptionProps(ActiveMessageCount: 34, DeadLetterMessageCount: 78));

            var result = await Seba().Execute(new[] {"subscription", "props", "topic69/sub1"});

            AssertSuccess(result);
            AssertConsoleOutput(
                "{\"activeMessageCount\":34,\"deadLetterMessageCount\":78}");
        }
        
        [Fact]
        public async Task Full_subscription_name_argument_is_required()
        {
            var result = await Seba().Execute(new[] {"subscription", "props"});

            AssertFailure(result, "The Full subscription name field is required.");
        }
        
        [Theory]
        [ClassData(typeof(InvalidFullSubscriptionName))]
        public async Task Invalid_full_subscription_name_format_is_rejected(string invalidFullSubscriptionName)
        {
            var result = await Seba().Execute(new[] {"subscription", "props", invalidFullSubscriptionName});

            AssertFailure(result, "Full subscription name must be provided in following format <topic name>/<subscription name>");
        }
    }
}