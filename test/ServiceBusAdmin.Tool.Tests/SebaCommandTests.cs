using System;
using FluentAssertions;
using Moq;
using ServiceBusAdmin.Client;
using ServiceBusAdmin.Tool.Options;

namespace ServiceBusAdmin.Tool.Tests
{
    public abstract class SebaCommandTests
    {
        protected readonly TestConsole Console = new ();
        protected readonly Mock<IServiceBusClient> Client = new(MockBehavior.Strict);

        protected void AssertSuccess(SebaResult result)
        {
            Console.ErrorText.Should().BeEmpty();
            result.Should().Be(SebaResult.Success);
        }
        
        protected void AssertFailure(SebaResult result, params string[] errorLines)
        {
            Console.ErrorText.Should().StartWith(string.Join(Environment.NewLine, errorLines));
            result.Should().Be(SebaResult.Failure);
        }

        protected void AssertConsoleOutput(params string[] lines)
        {
            Console.OutputText.Should().StartWith(string.Join(Environment.NewLine, lines));
        }

        protected Seba Seba()
        {
            const string connectionStringValue = "secretConnectionString";
            static string? GetEnvironmentVariable(string variableName) =>
                variableName == ConnectionStringOption.EnvironmentVariableName ? connectionStringValue : null;
            IServiceBusClient CreateServiceBusClient(string connectionString) =>
                connectionString == connectionStringValue
                    ? Client.Object
                    : throw new ArgumentException(connectionString);

            return new Seba(Console, CreateServiceBusClient, GetEnvironmentVariable);
        }
    }
}