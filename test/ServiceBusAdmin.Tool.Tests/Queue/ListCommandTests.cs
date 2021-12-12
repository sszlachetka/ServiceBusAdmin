using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceBusAdmin.CommandHandlers.Queue.List;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests.Queue
{
    public class ListCommandTests: SebaCommandTests
    {
        [Fact]
        public async Task Lists_all_queues()
        {
            Mediator.Setup<ListQueues, IReadOnlyCollection<string>>(new[] {"q1", "q2", "q3", "q4"});

            var result = await Seba().Execute(new[] {"queue", "list"});

            AssertSuccess(result);
            AssertConsoleOutput("q1", "q2", "q3", "q4");
        }
    }
}