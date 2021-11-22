using System;
using System.Threading.Tasks;
using FluentAssertions;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ServiceBusAdmin.Tool.Options;

namespace ServiceBusAdmin.Tool.Tests
{
    public abstract class SebaCommandTests : IAsyncDisposable
    {
        private readonly ServiceProvider _serviceProvider;
        protected readonly TestConsole Console = new ();
        protected readonly Mock<IMediator> Mediator = new(MockBehavior.Strict);

        protected SebaCommandTests()
        {
            var services = ConfigureServices(Console, Mediator.Object);
            _serviceProvider = services.BuildServiceProvider();
        }

        protected void AssertSuccess(SebaResult result)
        {
            Console.ErrorText.Should().BeEmpty();
            result.Should().Be(SebaResult.Success);
        }
        
        protected void AssertFailure(SebaResult result, params string[] errorLines)
        {
            Console.ErrorText.Should().Be(ConsoleOutput(errorLines));
            result.Should().Be(SebaResult.Failure);
        }

        protected void AssertConsoleOutput(params string[] lines)
        {
            Console.OutputText.Should().Be(ConsoleOutput(lines));
        }

        private static string ConsoleOutput(string[] lines)
        {
            return string.Join(Environment.NewLine, lines) + Environment.NewLine;
        }

        protected Seba Seba()
        {
            return _serviceProvider.GetRequiredService<Seba>();
        }

        private static IServiceCollection ConfigureServices(IConsole console, IMediator mediator)
        {
            const string connectionStringValue = "secretConnectionString";
            static string? GetEnvironmentVariable(string variableName) =>
                variableName == ConnectionString.EnvironmentVariableName ? connectionStringValue : null;

            var services = new ServiceCollection();
            services.AddSingleton(mediator);
            services.AddSeba(console, GetEnvironmentVariable);

            return services;
        }

        public ValueTask DisposeAsync()
        {
            return _serviceProvider.DisposeAsync();
        }
    }
}