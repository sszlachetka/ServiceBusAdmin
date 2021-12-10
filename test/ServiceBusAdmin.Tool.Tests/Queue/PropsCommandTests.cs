using System.Threading.Tasks;
using ServiceBusAdmin.CommandHandlers.Queue.Props;
using ServiceBusAdmin.Tool.Tests.TestData;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests.Queue
{
    public class PropsCommandTests: SebaCommandTests
    {
        [Fact]
        public async Task Returns_queue_properties()
        {
            Mediator.Setup<GetQueueProps, QueueProps>(
                new QueueProps(ActiveMessageCount: 34, DeadLetterMessageCount: 78));

            var result = await Seba().Execute(new[] {"queue", "props", "q1"});

            AssertSuccess(result);
            AssertConsoleOutput(
                "{\"activeMessageCount\":34,\"deadLetterMessageCount\":78}");
        }
        
        [Fact]
        public async Task Queue_name_argument_is_required()
        {
            var result = await Seba().Execute(new[] {"queue", "props"});

            AssertFailure(result, "The Queue name field is required.");
        }
    }
}