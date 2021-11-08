using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using McMaster.Extensions.CommandLineUtils;
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
            var console = new TestConsole();
            static string? GetEnvironmentVariable(string variableName) =>
                variableName == "SEBA_CONNECTION_STRING" ? "secretConnectionString" : null;
            var seba = new Seba(CreateServiceBusClient, console, GetEnvironmentVariable);

            var result = await seba.Execute(new[] {"props"});

            console.ErrorText.Should().BeEmpty();
            result.Should().Be(SebaResult.Success);
            console.OutputText.Should().Be("dsds");
        }
    }

    internal class TestConsole : IConsole
    {
        private readonly StringBuilder _output = new();
        private readonly StringBuilder _error = new();

        public string OutputText => _output.ToString();
        public string ErrorText => _error.ToString();

        public TestConsole()
        {
            Out = new StringWriter(_output);
            Error = new StringWriter(_error);
        }

        public void ResetColor()
        {
        }

        public TextWriter Out { get; }
        public TextWriter Error { get; }
        public TextReader In => throw new NotImplementedException();
        public bool IsInputRedirected => throw new NotImplementedException();
        public bool IsOutputRedirected => true;
        public bool IsErrorRedirected => true;
        public ConsoleColor ForegroundColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }
        public event ConsoleCancelEventHandler? CancelKeyPress;
    }
}