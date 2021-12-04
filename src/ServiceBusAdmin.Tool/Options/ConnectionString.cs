using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.CommandHandlers;

namespace ServiceBusAdmin.Tool.Options
{
    public delegate string? GetEnvironmentVariable(string variableName);

    public static class ConnectionString
    {
        public const string EnvironmentVariableName = "SEBA_CONNECTION_STRING";
        private const string Template = "-c|--connection-string";

        public static GetServiceBusConnectionString ConfigureConnectionStringOption(this CommandLineApplication application,
            GetEnvironmentVariable getEnvironmentVariable)
        {
            var connectionStringOption =
                application.Option<string?>(Template,
                    $"Azure Service Bus Connection string. Can be also provided with {EnvironmentVariableName} environment variable.",
                    CommandOptionType.SingleValue, inherited: true);

            return () => GetConnectionString(connectionStringOption.ParsedValue, getEnvironmentVariable);
        }

        private static string GetConnectionString(string? connectionString, GetEnvironmentVariable getEnvironmentVariable)
        {
            return connectionString
                   ?? getEnvironmentVariable(EnvironmentVariableName)
                   ?? throw new SebaCommandParsingException(
                       $"Azure Service Bus connection string must be provided either with {Template} option or with {EnvironmentVariableName} environment variable");
        }
    }
}