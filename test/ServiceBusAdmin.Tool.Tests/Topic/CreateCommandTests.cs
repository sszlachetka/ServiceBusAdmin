using System.Threading.Tasks;
using ServiceBusAdmin.CommandHandlers.Topic.Create;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests.Topic
{
    public class CreateCommandTests : SebaCommandTests
    {
        [Fact]
        public async Task Creates_topic_with_provided_name()
        {
            Mediator.Setup<CreateTopic>();

            var result = await Seba().Execute(new[] {"topic", "create", "topic69"});

            AssertSuccess(result);
            Mediator.VerifyOnce(new CreateTopic("topic69"));
        }
        
        [Fact]
        public async Task Topic_name_argument_is_required()
        {
            var result = await Seba().Execute(new[] {"topic", "create"});

            AssertFailure(result, "The Topic name field is required.");
        }
    }
}