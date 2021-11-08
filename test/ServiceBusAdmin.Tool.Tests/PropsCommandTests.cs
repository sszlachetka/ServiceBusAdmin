using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using ServiceBusAdmin.Client;
using ServiceBusAdmin.Tool.Options;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests
{
    public class PropsCommandTests
    {
        private readonly TestConsole _console = new ();
        private readonly Mock<IServiceBusClient> _client = new(MockBehavior.Strict);

        [Fact]
        public async Task Returns_service_bus_namespace_name()
        {
            _client.Setup(x => x.GetNamespaceName(It.IsAny<CancellationToken>()))
                .ReturnsAsync("test-namespace");

            var result = await Seba().Execute(new[] {"props"});

            AssertIsSuccess(result);
            _console.OutputText.Should().Be($"Namespace\ttest-namespace{Environment.NewLine}");
        }

        private void AssertIsSuccess(SebaResult result)
        {
            _console.ErrorText.Should().BeEmpty();
            result.Should().Be(SebaResult.Success);
        }

        private Seba Seba()
        {
            const string connectionStringValue = "secretConnectionString";
            static string? GetEnvironmentVariable(string variableName) =>
                variableName == ConnectionStringOption.EnvironmentVariableName ? connectionStringValue : null;
            IServiceBusClient CreateServiceBusClient(string connectionString) =>
                connectionString == connectionStringValue
                    ? _client.Object
                    : throw new ArgumentException(connectionString);

            return new Seba(_console, CreateServiceBusClient, GetEnvironmentVariable);
        }
    }
}