using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests.Topic
{
    public class ListCommandTests : SebaCommandTests
    {
        [Fact]
        public async Task Lists_all_topics()
        {
            Client.Setup(x => x.GetTopicsNames(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new []{"topic1", "topic2", "topic3"});

            var result = await Seba().Execute(new[] {"topic", "list"});

            AssertSuccess(result);
            AssertConsoleOutput("topic1", "topic2", "topic3");
        }
    }
}