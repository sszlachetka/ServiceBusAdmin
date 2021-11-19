using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests.Subscription.Receive
{
    public class ReceiveSubCommandsTests : SebaCommandTests
    {
        [Theory]
        [ClassData(typeof(ReceiveSubCommands))]
        public async Task Full_subscription_name_is_required(string subCommand)
        {
            var result = await Seba().Execute(new[] {"subscription", "receive", subCommand});

            AssertFailure(result, "The Full subscription name field is required.");
        }
        
        [Theory]
        [ClassData(typeof(ReceiveSubCommands))]
        public async Task Supports_max_option(string subCommand)
        {
            var options = new ReceiverOptionsBuilder()
                .WithMaxMessages(51)
                .Build();
            Mediator.SetupNoReceiveMessages(options);

            await Seba().Execute(new[]
                {"subscription", "receive", subCommand, "someTopic/someSubscription", "--max", "51"});

            Mediator.VerifyReceiveMessagesOnce(options);
        }
        
        [Theory]
        [ClassData(typeof(ReceiveSubCommands))]
        public async Task Supports_message_handling_concurrency_level_option(string subCommand)
        {
            var options = new ReceiverOptionsBuilder()
                .WithMessageHandlingConcurrencyLevel(76)
                .Build();
            Mediator.SetupNoReceiveMessages(options);

            await Seba().Execute(new[]
                {"subscription", "receive", subCommand, "someTopic/someSubscription", "--message-handling-concurrency-level", "76"});

            Mediator.VerifyReceiveMessagesOnce(options);
        }
        
        [Theory]
        [ClassData(typeof(ReceiveSubCommandsSupportingDlq))]
        public async Task Supports_dead_letter_queue_option(string subCommand)
        {
            var options = new ReceiverOptionsBuilder()
                .WithIsDeadLetterSubQueue(true)
                .Build();
            Mediator.SetupNoReceiveMessages(options);

            await Seba().Execute(new[]
                {"subscription", "receive", subCommand, "someTopic/someSubscription", "--dead-letter-queue"});

            Mediator.VerifyReceiveMessagesOnce(options);
        }
    }

    public class ReceiveSubCommandsSupportingDlq : TheoryData<string>
    {
        public ReceiveSubCommandsSupportingDlq()
        {
            Add("console");
            Add("send-to-topic");
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