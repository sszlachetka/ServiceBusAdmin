using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests.Topic
{
    public class CreateCommandTests : SebaCommandTests
    {
        public CreateCommandTests()
        {
            Client.Setup(x => x.CreateTopic(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
        }

        [Fact]
        public async Task Creates_topic_with_provided_name()
        {
            var result = await Seba().Execute(new[] {"topic", "create", "topic69"});

            AssertSuccess(result);
            Client.Verify(x => x.CreateTopic("topic69", It.IsAny<CancellationToken>()), Times.Once);
        }
        
        [Fact]
        public async Task Topic_name_argument_is_required()
        {
            var result = await Seba().Execute(new[] {"topic", "create"});

            AssertFailure(result, "The Topic name field is required.");
            Client.Verify(x => x.CreateTopic(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}