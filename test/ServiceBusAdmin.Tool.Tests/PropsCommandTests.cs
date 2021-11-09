using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests
{
    public class PropsCommandTests : SebaCommandTests
    {
        [Fact]
        public async Task Returns_service_bus_namespace_name()
        {
            Client.Setup(x => x.GetNamespaceName(It.IsAny<CancellationToken>()))
                .ReturnsAsync("test-namespace");

            var result = await Seba().Execute(new[] {"props"});

            AssertSuccess(result);
            AssertConsoleOutput("Namespace\ttest-namespace");
        }
    }
}