using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using ServiceBusAdmin.Client;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests.Subscription.Receive
{
    public class DeadLetterCommandTests : SebaCommandTests
    {
        [Fact]
        public async Task Prints_and_dead_letters_received_messages()
        {
            var messages = new[]
            {
                new TestMessageBuilder().WithSequenceNumber(5).Build(),
                new TestMessageBuilder().WithSequenceNumber(12).Build(),
            };
            var options = new ReceiverOptions(new ReceiverEntityName("topic3", "sub9"), 10);
            Client.SetupReceive(options, async handler =>
            {
                foreach (var message in messages)
                {
                    await handler(message);
                }
            });

            var result = await Seba().Execute(new[] {"subscription", "receive", "deadletter", "topic3/sub9"});

            AssertSuccess(result);
            AssertConsoleOutput("5", "12");
            messages[0].DeadLetteredOnce.Should().BeTrue();
            messages[1].DeadLetteredOnce.Should().BeTrue();
        }
        
        [Fact]
        public async Task Full_subscription_name_is_required()
        {
            var result = await Seba().Execute(new[] {"subscription", "receive", "deadletter"});

            AssertFailure(result, "The Full subscription name field is required.");
        }
        
        [Fact]
        public async Task Supports_max_option()
        {
            var options = new ReceiverOptions(new ReceiverEntityName("someTopic", "someSubscription"), 51);
            Client.SetupReceive(options, _ => Task.CompletedTask);

            await Seba().Execute(new[]
                {"subscription", "receive", "deadletter", "someTopic/someSubscription", "--max", "51"});

            Client.Verify(x => x.Receive(options, It.IsAny<ReceivedMessageHandler>()), Times.Once);
        }
    }
}