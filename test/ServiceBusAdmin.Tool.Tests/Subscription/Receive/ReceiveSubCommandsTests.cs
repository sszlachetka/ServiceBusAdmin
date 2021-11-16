using System.Linq;
using System.Threading.Tasks;
using Moq;
using ServiceBusAdmin.Client;
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
            Client.SetupReceive(options, _ => Task.CompletedTask);

            await Seba().Execute(new[]
                {"subscription", "receive", subCommand, "someTopic/someSubscription", "--max", "51"});

            Client.Verify(x => x.Receive(options, It.IsAny<ReceivedMessageHandler>()), Times.Once);
        }
        
        [Theory]
        [ClassData(typeof(ReceiveSubCommandsSupportingDlq))]
        public async Task Supports_dead_letter_queue_option(string subCommand)
        {
            var options = new ReceiverOptionsBuilder()
                .WithIsDeadLetterSubQueue(true)
                .Build();
            Client.SetupReceive(options, _ => Task.CompletedTask);

            await Seba().Execute(new[]
                {"subscription", "receive", subCommand, "someTopic/someSubscription", "--dead-letter-queue"});

            Client.Verify(x => x.Receive(options, It.IsAny<ReceivedMessageHandler>()), Times.Once);
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