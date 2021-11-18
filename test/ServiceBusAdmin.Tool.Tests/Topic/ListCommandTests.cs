using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceBusAdmin.CommandHandlers.Topic.List;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests.Topic
{
    public class ListCommandTests : SebaCommandTests
    {
        [Fact]
        public async Task Lists_all_topics()
        {
            Mediator.Setup<ListTopics, IReadOnlyCollection<string>>(
                new[] { "topic1", "topic2", "topic3" });

            var result = await Seba().Execute(new[] {"topic", "list"});

            AssertSuccess(result);
            AssertConsoleOutput("topic1", "topic2", "topic3");
        }
    }
}