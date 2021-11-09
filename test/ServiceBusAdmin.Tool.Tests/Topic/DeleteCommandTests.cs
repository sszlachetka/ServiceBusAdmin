using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests.Topic
{
    public class DeleteCommandTests : SebaCommandTests
    {
        public DeleteCommandTests()
        {
            Client.Setup(x => x.DeleteTopic(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
        }

        [Fact]
        public async Task Deletes_topic_with_provided_name()
        {
            var result = await Seba().Execute(new[] {"topic", "delete", "topic69"});

            AssertSuccess(result);
            Client.Verify(x => x.DeleteTopic("topic69", It.IsAny<CancellationToken>()), Times.Once);
        }
        
        [Fact]
        public async Task Topic_name_argument_is_required()
        {
            var result = await Seba().Execute(new[] {"topic", "delete"});

            AssertFailure(result, "The Topic name field is required.");
            Client.Verify(x => x.DeleteTopic(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}