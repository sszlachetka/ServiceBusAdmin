using System.Threading.Tasks;
using ServiceBusAdmin.CommandHandlers.Queue.Delete;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests.Queue
{
    public class DeleteCommandTests : SebaCommandTests
    {
        [Fact]
        public async Task Deletes_queue_with_provided_name()
        {
            Mediator.Setup<DeleteQueue>();

            var result = await Seba().Execute(new[] {"queue", "delete", "queue69"});

            AssertSuccess(result);
            Mediator.VerifyOnce(new DeleteQueue("queue69"));
        }
        
        [Fact]
        public async Task Queue_name_argument_is_required()
        {
            var result = await Seba().Execute(new[] {"queue", "delete"});

            AssertFailure(result, "The Queue name field is required.");
        }
    }
}