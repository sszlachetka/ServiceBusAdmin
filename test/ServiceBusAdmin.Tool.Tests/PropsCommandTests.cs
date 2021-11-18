using System;
using System.Threading.Tasks;
using ServiceBusAdmin.CommandHandlers.Props;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests
{
    public class PropsCommandTests : SebaCommandTests
    {
        [Fact]
        public async Task Returns_service_bus_namespace_properties()
        {
            Mediator.Setup<GetNamespaceProps, NamespaceProps>(
                new NamespaceProps(
                    "test-namespace", 
                    new DateTimeOffset(2021, 10, 15, 5, 30, 21, TimeSpan.Zero),
                    new DateTimeOffset(2021, 9, 17, 3, 20, 59, TimeSpan.FromHours(1))));

            var result = await Seba().Execute(new[] {"props"});

            AssertSuccess(result);
            AssertConsoleOutput(
                "{\"NamespaceName\":\"test-namespace\",\"CreatedTime\":\"2021-10-15T05:30:21+00:00\",\"ModifiedTime\":\"2021-09-17T03:20:59+01:00\"}");
        }
    }
}