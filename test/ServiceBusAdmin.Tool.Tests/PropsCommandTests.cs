using System;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using ServiceBusAdmin.Client;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests
{
    public class PropsCommandTests
    {
        [Fact]
        public async Task Returns_service_bus_namespace_name()
        {
            var client = new Mock<IServiceBusClient>(MockBehavior.Strict);
            IServiceBusClient CreateServiceBusClient(string connectionString) =>
                connectionString == "secretConnectionString"
                    ? client.Object
                    : throw new ArgumentException(connectionString);
            var outputWriter = new OutputWriterMock();
            static string? GetEnvironmentVariable(string variableName) =>
                variableName == "SEBA_CONNECTION_STRING" ? "secretConnectionString" : null;
            var seba = new Seba(CreateServiceBusClient, outputWriter, GetEnvironmentVariable);

            var result = await seba.Execute(new[] {"props"});

            result.Should().Be(SebaResult.Success);

            outputWriter.Output.Should().Be("dsds");
        }
    }

    internal class OutputWriterMock : IOutputWriter
    {
        private readonly StringBuilder _output = new();
        private readonly StringBuilder _error = new();

        public string Output => _output.ToString();
        public string Error => _error.ToString();
        
        public void WriteLine(string message)
        {
            _output.AppendLine(message);
        }

        public void WriteLine(string format, params object[] args)
        {
            _output.AppendFormat(format, args);
        }

        public void WriteErrorLine(string message)
        {
            _error.AppendLine(message);
        }
    }
}