using System.Threading.Tasks;
using FluentAssertions;
using Moq;
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
            Client.SetupReceive(options, async handler =>
            {
                foreach (var message in messages)
                {
                    await handler(message);
                }
            });

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
        
        [Fact]
        public async Task Full_subscription_name_is_required()
        {
            var result = await Seba().Execute(new[] {"subscription", "receive", "console"});

            AssertFailure(result, "The Full subscription name field is required.");
        }
        
        [Fact]
        public async Task Supports_max_option()
        {
            var options = new ReceiverOptionsBuilder()
                .WithMaxMessages(51)
                .Build();
            Client.SetupReceive(options, _ => Task.CompletedTask);

            await Seba().Execute(new[]
                {"subscription", "receive", "console", "someTopic/someSubscription", "--max", "51"});

            Client.Verify(x => x.Receive(options, It.IsAny<ReceivedMessageHandler>()), Times.Once);
        }
        
        [Fact]
        public async Task Supports_dead_letter_queue_option()
        {
            var options = new ReceiverOptionsBuilder()
                .WithIsDeadLetterSubQueue(true)
                .Build();
            Client.SetupReceive(options, _ => Task.CompletedTask);

            await Seba().Execute(new[]
                {"subscription", "receive", "console", "someTopic/someSubscription", "--dead-letter-queue"});

            Client.Verify(x => x.Receive(options, It.IsAny<ReceivedMessageHandler>()), Times.Once);
        }
    }
}