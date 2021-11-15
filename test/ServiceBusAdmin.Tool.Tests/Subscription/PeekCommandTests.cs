using System;
using System.Threading.Tasks;
using Moq;
using ServiceBusAdmin.Client;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests.Subscription
{
    public class PeekCommandTests : SebaCommandTests
    {
        [Fact]
        public async Task Peeks_messages()
        {
            var options = new ReceiverOptions(new ReceiverEntityName("topic77", "sub34"), 10);
            Client.SetupPeek(options, async handler =>
            {
                await handler(new TestMessageBuilder().WithBody("{\"key1\":12}").Build());
                await handler(new TestMessageBuilder().WithBody("{\"key2\":45}").Build());
            });

            var result = await Seba().Execute(new[] {"subscription", "peek", "topic77/sub34"});

            AssertSuccess(result);
            AssertConsoleOutput("{\"key1\":12}", "{\"key2\":45}");
        }

        [Fact]
        public async Task Returns_messages_in_provided_format()
        {
            var options = new ReceiverOptions(new ReceiverEntityName("topic56", "sub4"), 10);
            Client.SetupPeek(options, handler =>
                handler(new TestMessageBuilder()
                    .WithBody("{\"key1\":99}")
                    .WithSequenceNumber(89)
                    .WithMessageId("someMessageId")
                    .WithApplicationProperty("prop1", "value1")
                    .Build()));

            var result = await Seba().Execute(new[]
                {"subscription", "peek", "topic56/sub4", "--output-format", "{0} {1} {2} {3}"});

            AssertSuccess(result);
            AssertConsoleOutput("{\"key1\":99} 89 someMessageId {\"prop1\":\"value1\"}");
        }
        
        [Fact]
        public async Task Full_subscription_name_is_required()
        {
            var result = await Seba().Execute(new[] {"subscription", "peek"});

            AssertFailure(result, "The Full subscription name field is required.");
        }

        [Fact]
        public async Task Supports_max_option()
        {
            var options = new ReceiverOptions(new ReceiverEntityName("someTopic", "someSubscription"), 101);
            Client.SetupPeek(options, _ => Task.CompletedTask);
            
            await Seba().Execute(new[]
                {"subscription", "peek", "someTopic/someSubscription", "--max", "101"});

            Client.Verify(x => x.Peek(options, It.IsAny<MessageHandler>()), Times.Once);
        }
    }
}