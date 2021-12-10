using System.Threading.Tasks;
using ServiceBusAdmin.CommandHandlers.Queue.Create;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests.Queue
{
    public class CreateCommandTests : SebaCommandTests
    {
        [Fact]
        public async Task Creates_queue_with_provided_name()
        {
            Mediator.Setup<CreateQueue>();

            var result = await Seba().Execute(new[] {"queue", "create", "queue1"});

            AssertSuccess(result);
            Mediator.VerifyOnce(new CreateQueue("queue1"));
        }

        [Fact]
        public async Task Queue_name_argument_is_required()
        {
            var result = await Seba().Execute(new[] {"queue", "create"});

            AssertFailure(result, "The Queue name field is required.");
        }
    }
}