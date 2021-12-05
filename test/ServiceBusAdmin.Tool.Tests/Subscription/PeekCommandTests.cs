using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using ServiceBusAdmin.CommandHandlers;
using ServiceBusAdmin.CommandHandlers.Models;
using ServiceBusAdmin.CommandHandlers.Subscription.Peek;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests.Subscription
{
    public class PeekCommandTests : SebaCommandTests
    {
        [Fact]
        public async Task Returns_message_metadata_by_default()
        {
            IMessage[] messages = {
                new TestMessageBuilder().WithSequenceNumber(32).Build(),
                new TestMessageBuilder().WithSequenceNumber(99).Build()
            };
            var options = new ReceiverOptionsBuilder()
                .WithEntityName(new ReceiverEntityName("topic77", "sub34"))
                .Build();
            Mediator.SetupPeekMessages(options, messages);

            var result = await Seba().Execute(new[] {"subscription", "peek", "topic77/sub34"});

            AssertSuccess(result);
            AssertConsoleOutputContainJsonSubtrees(
                "{\"sequenceNumber\":32}",
                "{\"sequenceNumber\":99}");
        }

        [Fact]
        public async Task Returns_message_body_when_requested()
        {
            var options = new ReceiverOptionsBuilder()
                .WithEntityName(new ReceiverEntityName("topic56", "sub4"))
                .Build();
            Mediator.SetupPeekMessages(options, new TestMessageBuilder()
                .WithBody("{\"key1\":99}")
                .Build());
        
            var result = await Seba().Execute(new[]
                {"subscription", "peek", "topic56/sub4", "--output-content", "body"});
        
            AssertSuccess(result);
            AssertConsoleOutput("{\"key1\":99}");
        }
        
        [Fact]
        public async Task Returns_message_body_and_metadata_when_requested()
        {
            var options = new ReceiverOptionsBuilder()
                .WithEntityName(new ReceiverEntityName("topic56", "sub4"))
                .Build();
            Mediator.SetupPeekMessages(options, new TestMessageBuilder().Build());
        
            var result = await Seba().Execute(new[]
                {"subscription", "peek", "topic56/sub4", "--output-content", "all"});
        
            AssertSuccess(result);
            AssertConsoleOutputEachLineShouldHaveJsonElement("body");
            AssertConsoleOutputEachLineShouldHaveJsonElement("metadata");
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
            var options = new ReceiverOptionsBuilder()
                .WithMaxMessages(101)
                .Build();
            Mediator.SetupPeekMessages(options);
            
            await Seba().Execute(new[]
                {"subscription", "peek", "someTopic/someSubscription", "--max", "101"});

            Mediator.VerifyPeekMessagesOnce(options);
        }
        
        [Fact]
        public async Task Supports_dead_letter_queue_option()
        {
            var options = new ReceiverOptionsBuilder()
                .WithIsDeadLetterSubQueue(true)
                .Build();
            Mediator.SetupPeekMessages(options);
        
            await Seba().Execute(new[]
                {"subscription", "peek", "someTopic/someSubscription", "--dead-letter-queue"});
        
            Mediator.VerifyPeekMessagesOnce(options);
        }
        
        [Fact]
        public async Task Supports_message_handling_concurrency_level_option()
        {
            var options = new ReceiverOptionsBuilder()
                .WithMessageHandlingConcurrencyLevel(21)
                .Build();
            Mediator.SetupPeekMessages(options);
        
            await Seba().Execute(new[]
                {"subscription", "peek", "someTopic/someSubscription", "--message-handling-concurrency-level", "21"});
        
            Mediator.VerifyPeekMessagesOnce(options);
        }
        
        [Fact]
        public async Task Supports_from_sequence_number_option()
        {
            var options = new ReceiverOptionsBuilder().Build();
            Mediator.SetupPeekMessages(options);
        
            await Seba().Execute(new[]
                {"subscription", "peek", "someTopic/someSubscription", "--from-sequence-number", "101"});
        
            Mediator.VerifyPeekMessagesOnce(options, fromSequenceNumber: 101);
        }
    }
    
    internal static class PeekMessagesMediatorMockExtensions
    {
        public static void SetupPeekMessages(this Mock<IMediator> mock, ReceiverOptions options, params IMessage[] messages)
        {
            mock.Setup<PeekMessages>(
                request => request.Options == options,
                async peekMessages =>
                {
                    foreach (var message in messages)
                    {
                        await peekMessages.Callback(message);
                    }
                });
        }
        
        public static void VerifyPeekMessagesOnce(this Mock<IMediator> mock, ReceiverOptions options, long? fromSequenceNumber = null)
        {
            mock.Verify(
                x => x.Send(
                    It.Is<PeekMessages>(request => 
                        request.Options == options &&
                        request.FromSequenceNumber == fromSequenceNumber),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}