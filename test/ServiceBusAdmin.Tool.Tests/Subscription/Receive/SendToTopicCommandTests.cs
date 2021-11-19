using System;
using System.Threading.Tasks;
using FluentAssertions;
using ServiceBusAdmin.CommandHandlers;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests.Subscription.Receive
{
    public class SendToTopicCommandTests : SebaCommandTests
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
            Mediator.SetupReceiveMessages(options, messages);
            Mediator.SetupSendAnyBinaryMessage();

            var result = await Seba().Execute(new[] {"subscription", "receive", "send-to-topic", "topic1/sub2"});

            AssertSuccess(result);
            AssertConsoleOutput("3", "5", "9");
            messages[0].CompletedOnce.Should().BeTrue();
            messages[1].CompletedOnce.Should().BeTrue();
            messages[2].CompletedOnce.Should().BeTrue();
            Mediator.VerifySendBinaryMessageOnce("topic1", messages[0].Body);
            Mediator.VerifySendBinaryMessageOnce("topic1", messages[1].Body);
            Mediator.VerifySendBinaryMessageOnce("topic1", messages[2].Body);
        }
    }
}