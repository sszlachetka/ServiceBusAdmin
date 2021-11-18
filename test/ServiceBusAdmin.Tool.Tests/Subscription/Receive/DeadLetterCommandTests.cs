using System.Threading.Tasks;
using FluentAssertions;
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
                new TestMessageBuilder2().WithSequenceNumber(5).Build(),
                new TestMessageBuilder2().WithSequenceNumber(12).Build(),
            };
            var options = new ReceiverOptionsBuilder2()
                .WithEntityName(new ReceiverEntityName2("topic3", "sub9"))
                .Build();
            Client.SetupReceive(options, messages);

            var result = await Seba().Execute(new[] {"subscription", "receive", "dead-letter", "topic3/sub9"});

            AssertSuccess(result);
            AssertConsoleOutput("5", "12");
            messages[0].DeadLetteredOnce.Should().BeTrue();
            messages[1].DeadLetteredOnce.Should().BeTrue();
        }

        [Fact]
        public async Task Does_not_support_dead_letter_queue_option()
        {
            var result = await Seba().Execute(new[]
                {"subscription", "receive", "dead-letter", "someTopic/someSubscription", "--dead-letter-queue"});

            AssertFailure(result, "Unrecognized option '--dead-letter-queue'");
        }
    }
}