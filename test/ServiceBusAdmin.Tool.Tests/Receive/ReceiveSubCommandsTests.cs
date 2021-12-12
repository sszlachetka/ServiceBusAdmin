using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using ServiceBusAdmin.CommandHandlers;
using ServiceBusAdmin.CommandHandlers.Models;
using ServiceBusAdmin.Tool.Tests.Subscription;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests.Receive
{
    public class ReceiveSubCommandsTests : SebaCommandTests
    {
        [Theory]
        [ClassData(typeof(ReceiveSubCommands))]
        public async Task Receives_messages_from_queue(string subCommand)
        {
            var options = new ReceiverOptionsBuilder()
                .WithEntityName(new ReceiverEntityName("queue1"))
                .Build();
            Mediator.SetupReceiveMessages(options);
            Mediator.SetupSendAnyMessage();

            var result = await Seba().Execute(new[]
                { "receive", subCommand, "queue1" });

            AssertSuccess(result);
            Mediator.VerifyReceiveMessagesOnce(options);
        }
        
        [Theory]
        [ClassData(typeof(ReceiveSubCommands))]
        public async Task Receives_messages_from_subscription(string subCommand)
        {
            var options = new ReceiverOptionsBuilder()
                .WithEntityName(new ReceiverEntityName("topic1", "sub1"))
                .Build();
            Mediator.SetupReceiveMessages(options);
            Mediator.SetupSendAnyMessage();

            var result = await Seba().Execute(new[]
                { "receive", subCommand, "topic1/sub1" });

            AssertSuccess(result);
            Mediator.VerifyReceiveMessagesOnce(options);
        }

        [Theory]
        [ClassData(typeof(ReceiveSubCommands))]
        public async Task Queue_or_full_subscription_name_is_required(string subCommand)
        {
            var result = await Seba().Execute(new[] {"receive", subCommand});

            AssertFailure(result, "The Queue or full subscription name field is required.");
        }
        
        [Theory]
        [ClassData(typeof(ReceiveSubCommands))]
        public async Task Supports_max_option(string subCommand)
        {
            var options = new ReceiverOptionsBuilder()
                .WithMaxMessages(51)
                .Build();
            Mediator.SetupReceiveMessages(options);

            await Seba().Execute(new[]
                {"receive", subCommand, "someTopic/someSubscription", "--max", "51"});

            Mediator.VerifyReceiveMessagesOnce(options);
        }
        
        [Theory]
        [ClassData(typeof(ReceiveSubCommands))]
        public async Task Supports_message_handling_concurrency_level_option(string subCommand)
        {
            var options = new ReceiverOptionsBuilder()
                .WithMessageHandlingConcurrencyLevel(76)
                .Build();
            Mediator.SetupReceiveMessages(options);

            await Seba().Execute(new[]
                {"receive", subCommand, "someTopic/someSubscription", "--message-handling-concurrency-level", "76"});

            Mediator.VerifyReceiveMessagesOnce(options);
        }
        
        [Theory]
        [ClassData(typeof(ReceiveSubCommands))]
        public async Task Supports_handle_sequence_numbers_option(string subCommand)
        {
            var messages = new[]
            {
                new TestMessageBuilder().WithSequenceNumber(3).Build(),
                new TestMessageBuilder().WithSequenceNumber(4).Build(),
                new TestMessageBuilder().WithSequenceNumber(5).Build(),
                new TestMessageBuilder().WithSequenceNumber(6).Build(),
                new TestMessageBuilder().WithSequenceNumber(7).Build()
            };
            var options = new ReceiverOptionsBuilder().Build();
            Mediator.SetupReceiveMessages(options, messages.Cast<IReceivedMessage>().ToArray());
            Mediator.SetupSendAnyMessage();

            var result = await Seba().Execute(new[]
                {"receive", subCommand, "someTopic/someSubscription", "--handle-sequence-numbers", "3,5,7"});

            AssertSuccess(result);
            CompletedOrDeadLetteredOnce(messages[0], subCommand).Should().BeTrue();
            CompletedOrDeadLetteredOnce(messages[1], subCommand).Should().BeFalse();
            CompletedOrDeadLetteredOnce(messages[2], subCommand).Should().BeTrue();
            CompletedOrDeadLetteredOnce(messages[3], subCommand).Should().BeFalse();
            CompletedOrDeadLetteredOnce(messages[4], subCommand).Should().BeTrue();
        }
        
        [Theory]
        [ClassData(typeof(ReceiveSubCommands))]
        public async Task Verifies_that_all_expected_sequence_numbers_are_received(string subCommand)
        {
            var messages = new[]
            {
                new TestMessageBuilder().WithSequenceNumber(3).Build(),
                new TestMessageBuilder().WithSequenceNumber(6).Build(),
                new TestMessageBuilder().WithSequenceNumber(8).Build()
            };
            var options = new ReceiverOptionsBuilder().Build();
            Mediator.SetupReceiveMessages(options, messages.Cast<IReceivedMessage>().ToArray());
            Mediator.SetupSendAnyMessage();

            var result = await Seba().Execute(new[]
                {"receive", subCommand, "someTopic/someSubscription", "--handle-sequence-numbers", "3,5,7"});

            AssertFailure(result, "Following sequence numbers were not received: 5, 7");
        }
        
        private static bool CompletedOrDeadLetteredOnce(TestMessage message, string subCommand)
        {
            return subCommand == "dead-letter"
                ? message.DeadLetteredOnce
                : message.CompletedOnce;
        }

        [Theory]
        [ClassData(typeof(ReceiveSubCommands))]
        public async Task Prevents_handling_the_same_message_more_than_once(string subCommand)
        {
            var messages = new[]
            {
                new TestMessageBuilder().WithSequenceNumber(1).Build(),
                new TestMessageBuilder().WithSequenceNumber(2).Build(),
                new TestMessageBuilder().WithSequenceNumber(1).Build(),
            };
            var options = new ReceiverOptionsBuilder().Build();
            var exceptions = Mediator.SetupReceiveMessages(options, messages.Cast<IReceivedMessage>().ToArray());
            Mediator.SetupSendAnyMessage();

            await Seba().Execute(new[]
                {"receive", subCommand, "someTopic/someSubscription"});

            exceptions.Should().HaveCount(1).And
                .Subject.Single().Should().BeOfType<ApplicationException>().Which
                .Message.Should().StartWith("Message with sequence number 1 was received more than once");
        }

        [Theory]
        [ClassData(typeof(ReceiveSubCommandsSupportingDlq))]
        public async Task Supports_dead_letter_queue_option(string subCommand)
        {
            var options = new ReceiverOptionsBuilder()
                .WithIsDeadLetterSubQueue(true)
                .Build();
            Mediator.SetupReceiveMessages(options);

            await Seba().Execute(new[]
                {"receive", subCommand, "someTopic/someSubscription", "--dead-letter-queue"});

            Mediator.VerifyReceiveMessagesOnce(options);
        }
    }

    public class ReceiveSubCommandsSupportingDlq : TheoryData<string>
    {
        public ReceiveSubCommandsSupportingDlq()
        {
            Add("console");
            Add("send");
        }
    }

    public class ReceiveSubCommands : TheoryData<string>
    {
        public ReceiveSubCommands()
        {
            foreach (var values in new ReceiveSubCommandsSupportingDlq().AsEnumerable().ToList())
            {
                AddRow(values);
            }

            Add("dead-letter");
        }
    }
}