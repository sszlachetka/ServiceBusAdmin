using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using ServiceBusAdmin.CommandHandlers;
using ServiceBusAdmin.CommandHandlers.Models;
using ServiceBusAdmin.Tool.Tests.Subscription;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests.Receive
{
    public class SendCommandTests : SebaCommandTests
    {
        [Fact]
        public async Task Sends_received_messages_back_to_the_topic()
        {
            var messages = new[]
            {
                new TestMessageBuilder().WithSequenceNumber(3).Build(),
                new TestMessageBuilder().WithSequenceNumber(5).Build(),
                new TestMessageBuilder().WithSequenceNumber(9).Build()
            };
            var options = new ReceiverOptionsBuilder()
                .WithEntityName(new ReceiverEntityName("topic1", "sub2"))
                .Build();
            Mediator.SetupReceiveMessages(options, messages.Cast<IReceivedMessage>().ToArray());
            Mediator.SetupSendAnyMessage();

            var result = await Seba().Execute(new[] { "receive", "send", "topic1/sub2" });

            AssertSuccess(result);
            AssertConsoleOutput("3", "5", "9");
            messages[0].CompletedOnce.Should().BeTrue();
            messages[1].CompletedOnce.Should().BeTrue();
            messages[2].CompletedOnce.Should().BeTrue();
            Mediator.VerifySendMessageOnce("topic1", messages[0].Body);
            Mediator.VerifySendMessageOnce("topic1", messages[1].Body);
            Mediator.VerifySendMessageOnce("topic1", messages[2].Body);
        }
        
        [Fact]
        public async Task Sends_received_messages_back_to_the_queue()
        {
            var messages = new[]
            {
                new TestMessageBuilder().WithSequenceNumber(3).Build(),
                new TestMessageBuilder().WithSequenceNumber(5).Build(),
                new TestMessageBuilder().WithSequenceNumber(9).Build()
            };
            var options = new ReceiverOptionsBuilder()
                .WithEntityName(new ReceiverEntityName("queue58"))
                .Build();
            Mediator.SetupReceiveMessages(options, messages.Cast<IReceivedMessage>().ToArray());
            Mediator.SetupSendAnyMessage();

            var result = await Seba().Execute(new[] { "receive", "send", "queue58" });

            AssertSuccess(result);
            AssertConsoleOutput("3", "5", "9");
            messages[0].CompletedOnce.Should().BeTrue();
            messages[1].CompletedOnce.Should().BeTrue();
            messages[2].CompletedOnce.Should().BeTrue();
            Mediator.VerifySendMessageOnce("queue58", messages[0].Body);
            Mediator.VerifySendMessageOnce("queue58", messages[1].Body);
            Mediator.VerifySendMessageOnce("queue58", messages[2].Body);
        }
    }
}