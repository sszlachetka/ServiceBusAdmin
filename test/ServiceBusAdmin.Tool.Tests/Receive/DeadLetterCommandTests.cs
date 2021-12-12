using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using ServiceBusAdmin.CommandHandlers;
using ServiceBusAdmin.CommandHandlers.Models;
using ServiceBusAdmin.Tool.Tests.Subscription;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests.Receive
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
            var options = new ReceiverOptionsBuilder()
                .WithEntityName(new ReceiverEntityName("topic3", "sub9"))
                .Build();
            Mediator.SetupReceiveMessages(options, messages.Cast<IReceivedMessage>().ToArray());

            var result = await Seba().Execute(new[] {"receive", "dead-letter", "topic3/sub9"});

            AssertSuccess(result);
            AssertConsoleOutput("5", "12");
            messages[0].DeadLetteredOnce.Should().BeTrue();
            messages[1].DeadLetteredOnce.Should().BeTrue();
        }

        [Fact]
        public async Task Does_not_support_dead_letter_queue_option()
        {
            var result = await Seba().Execute(new[]
                {"receive", "dead-letter", "someTopic/someSubscription", "--dead-letter-queue"});

            AssertFailure(result, "Unrecognized option '--dead-letter-queue'");
        }
    }
}