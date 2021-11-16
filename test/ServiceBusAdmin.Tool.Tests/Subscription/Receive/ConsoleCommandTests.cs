using System.Threading.Tasks;
using FluentAssertions;
using ServiceBusAdmin.Client;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests.Subscription.Receive
{
    public class ConsoleCommandTests : SebaCommandTests
    {
        [Fact]
        public async Task Prints_and_completes_received_messages()
        {
            var messages = new[]
            {
                new TestMessageBuilder().WithBody("{\"key\":1}").Build(),
                new TestMessageBuilder().WithBody("{\"key\":2}").Build()
            };
            var options = new ReceiverOptionsBuilder()
                .WithEntityName(new ReceiverEntityName("topic1", "sub2"))
                .Build();
            Client.SetupReceive(options, messages);

            var result = await Seba().Execute(new[] {"subscription", "receive", "console", "topic1/sub2"});

            AssertSuccess(result);
            AssertConsoleOutput("{\"key\":1}", "{\"key\":2}");
            messages[0].CompletedOnce.Should().BeTrue();
            messages[1].CompletedOnce.Should().BeTrue();
        }
        
        [Fact]
        public async Task Returns_messages_in_provided_format()
        {
            var options = new ReceiverOptionsBuilder()
                .WithEntityName(new ReceiverEntityName("topic23", "sub7"))
                .Build();
            Client.SetupReceive(options, handler =>
                handler(new TestMessageBuilder()
                    .WithBody("{\"key1\":21}")
                    .WithSequenceNumber(9)
                    .WithMessageId("someMessageId")
                    .WithApplicationProperty("prop1", "value1")
                    .Build()));

            var result = await Seba().Execute(new[]
                {"subscription", "receive", "console", "topic23/sub7", "--output-format", "{0} {1} {2} {3}"});

            AssertSuccess(result);
            AssertConsoleOutput("{\"key1\":21} 9 someMessageId {\"prop1\":\"value1\"}");
        }
    }
}