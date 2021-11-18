using System.Threading.Tasks;
using ServiceBusAdmin.CommandHandlers.Topic.Delete;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests.Topic
{
    public class DeleteCommandTests : SebaCommandTests
    {
        [Fact]
        public async Task Deletes_topic_with_provided_name()
        {
            Mediator.Setup<DeleteTopic>();

            var result = await Seba().Execute(new[] {"topic", "delete", "topic69"});

            AssertSuccess(result);
            Mediator.VerifyOnce(new DeleteTopic("topic69"));
        }
        
        [Fact]
        public async Task Topic_name_argument_is_required()
        {
            var result = await Seba().Execute(new[] {"topic", "delete"});

            AssertFailure(result, "The Topic name field is required.");
        }
    }
}