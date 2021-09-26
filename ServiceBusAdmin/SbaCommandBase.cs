using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus.Administration;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin
{
    [HelpOption("-?|--help")]
    public class SbaCommandBase
    {
        private const string ConnectionStringEnvironmentVariable = "SBA_CONNECTION_STRING";
        private const string ConnectionStringOption = "-c|--connection-string";

        [Option(ConnectionStringOption)]
        public string ConnectionString { get; set; }

        protected virtual Task<int> OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();

            return Task.FromResult(1);
        }

        protected ServiceBusAdministrationClient AdministrationClient(CommandLineApplication app)
        {
            var connectionString = GetConnectionString(app);

            return new ServiceBusAdministrationClient(connectionString);
        }

        private string GetConnectionString(CommandLineApplication app)
        {
            return ConnectionString
                ?? Environment.GetEnvironmentVariable(ConnectionStringEnvironmentVariable)
                ?? throw new CommandParsingException(app, $"Azure Service Bus connection string must be provided either with {ConnectionStringOption} option or with {ConnectionStringEnvironmentVariable} environment variable");
        }
    }
}