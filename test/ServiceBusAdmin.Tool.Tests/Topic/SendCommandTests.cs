using System.Threading;
using System.Threading.Tasks;
using Moq;
using ServiceBusAdmin.CommandHandlers.Send;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests.Topic
{
    public class SendCommandTests : SebaCommandTests
    {
        [Fact]
        public async Task Sends_provided_message()
        {
            Mediator.Setup<SendMessage>();

            var result = await Seba().Execute(new[] {"topic", "send", "topic69", "--body", "message-body"});

            AssertSuccess(result);
            Mediator.VerifyOnce(new SendMessage("topic69", "message-body"));
        }
        
        [Fact]
        public async Task Topic_argument_is_required()
        {
            var result = await Seba().Execute(new[] {"topic", "send", "--body", "message-body"});

            AssertFailure(result, "The Topic name field is required.");
        }
        
        [Fact]
        public async Task Message_body_option_is_required()
        {
            var result = await Seba().Execute(new[] {"topic", "send", "topic69"});

            AssertFailure(result, "The --body field is required.");
        }
    }
}