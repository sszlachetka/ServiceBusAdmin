using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests.Topic
{
    public class SendCommandTests : SebaCommandTests
    {
        public SendCommandTests()
        {
            Client.Setup(x => x.SendMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
        }

        [Fact]
        public async Task Sends_provided_message()
        {
            var result = await Seba().Execute(new[] {"topic", "send", "topic69", "--body", "message-body"});

            AssertSuccess(result);
            Client.Verify(x => x.SendMessage("topic69", "message-body", It.IsAny<CancellationToken>()), Times.Once);
        }
        
        [Fact]
        public async Task Topic_argument_is_required()
        {
            var result = await Seba().Execute(new[] {"topic", "send", "--body", "message-body"});

            AssertFailure(result, "The Topic name field is required.");
            Client.Verify(x => x.SendMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }
        
        [Fact]
        public async Task Message_body_option_is_required()
        {
            var result = await Seba().Execute(new[] {"topic", "send", "topic69"});

            AssertFailure(result, "The --body field is required.");
            Client.Verify(x => x.SendMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}