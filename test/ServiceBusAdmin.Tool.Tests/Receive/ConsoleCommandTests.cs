using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using ServiceBusAdmin.CommandHandlers;
using ServiceBusAdmin.CommandHandlers.Models;
using ServiceBusAdmin.Tool.Tests.Subscription;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests.Receive
{
    public class ConsoleCommandTests : SebaCommandTests
    {
        [Fact]
        public async Task Returns_message_metadata_and_completes_received_messages()
        {
            TestMessage[] messages = {
                new TestMessageBuilder().WithSequenceNumber(21).Build(),
                new TestMessageBuilder().WithSequenceNumber(59).Build()
            };
            var options = new ReceiverOptionsBuilder()
                .WithEntityName(new ReceiverEntityName("topic77", "sub34"))
                .Build();
            Mediator.SetupReceiveMessages(options, messages.Cast<IReceivedMessage>().ToArray());

            var result = await Seba().Execute(new[] { "receive", "console", "topic77/sub34" });

            AssertSuccess(result);
            AssertConsoleOutputContainJsonSubtrees(
                "{\"sequenceNumber\":21}",
                "{\"sequenceNumber\":59}");
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
                {"receive", "console", "topic56/sub4", "--output-content", "body"});
        
            AssertSuccess(result);
            AssertConsoleOutput("{\"key1\":99}");
        }
        
        [Fact]
        public async Task Returns_message_body_and_metadata_when_requested()
        {
            var options = new ReceiverOptionsBuilder()
                .WithEntityName(new ReceiverEntityName("topic56", "sub4"))
                .Build();
            Mediator.SetupReceiveMessages(options, new TestMessageBuilder().Build());
        
            var result = await Seba().Execute(new[]
                {"receive", "console", "topic56/sub4", "--output-content", "all"});
        
            AssertSuccess(result);
            AssertConsoleOutputEachLineShouldHaveJsonElement("body");
            AssertConsoleOutputEachLineShouldHaveJsonElement("metadata");
        }
    }
}