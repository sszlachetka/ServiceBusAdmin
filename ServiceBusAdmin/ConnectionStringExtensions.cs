using System;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin
{
    public delegate string? GetEnvironmentVariable(string variableName);
    public delegate string GetConnectionString();

    public static class ConnectionStringExtensions
    {
        private const string ConnectionStringEnvironmentVariable = "SEBA_CONNECTION_STRING";
        private const string ConnectionStringOption = "-c|--connection-string";

        public static GetConnectionString ConfigureConnectionStringOption(this CommandLineApplication application,
            GetEnvironmentVariable getEnvironmentVariable)
        {
            var connectionStringOption =
                application.Option<string?>("-c|--connection-string",
                    $"Azure Service Bus Connection string. Can be also provided with {ConnectionStringEnvironmentVariable} environment variable.",
                    CommandOptionType.SingleValue, inherited: true);

            return () => GetConnectionString(connectionStringOption.ParsedValue, getEnvironmentVariable);
        }

        private static string GetConnectionString(string? connectionString, GetEnvironmentVariable getEnvironmentVariable)
        {
            return connectionString
                   ?? getEnvironmentVariable(ConnectionStringEnvironmentVariable)
                   ?? throw new SebaCommandParsingException(
                       $"Azure Service Bus connection string must be provided either with {ConnectionStringOption} option or with {ConnectionStringEnvironmentVariable} environment variable");
        }
    }
}