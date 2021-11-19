using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using ServiceBusAdmin.CommandHandlers;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests.Subscription.Receive
{
    public class ConsoleCommandTests : SebaCommandTests
    {
        [Fact]
        public async Task Returns_message_metadata_and_completes_received_messages()
        {
            TestMessage[] messages = {
                new TestMessageBuilder().WithMessageId("M1").WithSequenceNumber(1)
                    .WithApplicationProperty("Key1", 87).Build(),
                new TestMessageBuilder().WithMessageId("M2").WithSequenceNumber(2)
                    .WithApplicationProperty("Key2", "someValue").Build()
            };
            var options = new ReceiverOptionsBuilder()
                .WithEntityName(new ReceiverEntityName("topic77", "sub34"))
                .Build();
            Mediator.SetupReceiveMessages(options, messages.Cast<IReceivedMessage>().ToArray());

            var result = await Seba().Execute(new[] {"subscription", "receive", "console", "topic77/sub34"});

            AssertSuccess(result);
            AssertConsoleOutput(
                "{\"SequenceNumber\":1,\"MessageId\":\"M1\",\"ApplicationProperties\":{\"Key1\":87}}", 
                "{\"SequenceNumber\":2,\"MessageId\":\"M2\",\"ApplicationProperties\":{\"Key2\":\"someValue\"}}");
            messages[0].CompletedOnce.Should().BeTrue();
            messages[1].CompletedOnce.Should().BeTrue();
        }
        
        [Fact]
        public async Task Returns_messages_body_when_requested()
        {
            var options = new ReceiverOptionsBuilder()
                .WithEntityName(new ReceiverEntityName("topic56", "sub4"))
                .Build();
            Mediator.SetupReceiveMessages(options, new TestMessageBuilder()
                .WithBody("{\"key1\":99}")
                .Build());
        
            var result = await Seba().Execute(new[]
                {"subscription", "receive", "console", "topic56/sub4", "--output-content", "body"});
        
            AssertSuccess(result);
            AssertConsoleOutput("{\"key1\":99}");
        }
        
        [Fact]
        public async Task Returns_message_body_and_metadata_when_requested()
        {
            var options = new ReceiverOptionsBuilder()
                .WithEntityName(new ReceiverEntityName("topic56", "sub4"))
                .Build();
            Mediator.SetupReceiveMessages(options, new TestMessageBuilder()
                .WithBody("{\"key1\":99}")
                .WithMessageId("someId")
                .WithSequenceNumber(99)
                .WithApplicationProperty("Key1", 87)
                .Build());
        
            var result = await Seba().Execute(new[]
                {"subscription", "receive", "console", "topic56/sub4", "--output-content", "all"});
        
            AssertSuccess(result);
            AssertConsoleOutput(
                "{\"Body\":{\"key1\":99},\"SequenceNumber\":99,\"MessageId\":\"someId\",\"ApplicationProperties\":{\"Key1\":87}}");
        }
    }
}