using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.Client;
using ServiceBusAdmin.Tool.Subscription;
using ServiceBusAdmin.Tool.Topic;

namespace ServiceBusAdmin.Tool
{
    public delegate IServiceBusClient CreateServiceBusClientWith(string connectionString);

    public class Seba
    {
        public static async Task<int> Main(string[] args)
        {
            var seba = new Seba(
                connectionString => new SebaClient(connectionString),
                PhysicalConsole.Singleton,
                Environment.GetEnvironmentVariable);

            return (int) await seba.Execute(args);
        }

        private readonly CommandLineApplication _app;
        private readonly IConsole _console;

        public Seba(
            CreateServiceBusClientWith createServiceBusClient,
            IConsole console,
            GetEnvironmentVariable getEnvironmentVariable)
        {
            _console = console;
            _app = CreateApplication(console);
            var getConnectionString = _app.ConfigureConnectionStringOption(getEnvironmentVariable);
            var context = CreateContext(createServiceBusClient, console, getConnectionString);
            ConfigureCommands(_app, context);
        }

        public async Task<SebaResult> Execute(string[] args)
        {
            try
            {
                return (SebaResult) await _app.ExecuteAsync(args);
            }
            catch (Exception e)
            {
                await _console.Error.WriteLineAsync(e.Message);

                return SebaResult.Failure;
            }
        }

        private static CommandLineApplication CreateApplication(IConsole console)
        {
            var app = new CommandLineApplication(console)
            {
                Name = "seba",
                Description = "Azure Service Bus administration utility"
            };
            app.HelpOption(inherited: true);
            app.VersionOptionFromAssemblyAttributes("-v|--version", typeof(Seba).Assembly);
            app.OnExecute(() =>
            {
                app.ShowHelp();
                return (int) SebaResult.Failure;
            });

            return app;
        }

        private static SebaContext CreateContext(
            CreateServiceBusClientWith createServiceBusClient, 
            IConsole console,
            GetConnectionString getConnectionString)
        {
            IServiceBusClient CreateServiceBusClient() => createServiceBusClient(getConnectionString());

            return new SebaContext(CreateServiceBusClient, console);
        }

        private static void ConfigureCommands(CommandLineApplication app, SebaContext context)
        {
            app.Subcommand(new PropsCommand(context, app));
            app.Subcommand(new TopicCommand(context, app));
            app.Subcommand(new SubscriptionCommand(context, app));
        }
    }
}