using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.Client;
using ServiceBusAdmin.Tool.Options;
using ServiceBusAdmin.Tool.Subscription;
using ServiceBusAdmin.Tool.Topic;

namespace ServiceBusAdmin.Tool
{
    public delegate IServiceBusClient CreateServiceBusClient(string connectionString);

    public class Seba
    {
        public static async Task<int> Main(string[] args)
        {
            var seba = new Seba(
                PhysicalConsole.Singleton,
                connectionString => new SebaClient(connectionString),
                Environment.GetEnvironmentVariable);

            return (int) await seba.Execute(args);
        }

        private readonly CommandLineApplication _app;
        private readonly SebaConsole _console;

        public Seba(
            IConsole console,
            CreateServiceBusClient createServiceBusClient,
            GetEnvironmentVariable getEnvironmentVariable)
        {
            _app = CreateApplication(console);

            var isVerboseOutput = _app.ConfigureVerboseOption();
            _console = new SebaConsole(console, isVerboseOutput);

            var getConnectionString = _app.ConfigureConnectionStringOption(getEnvironmentVariable);
            var context = CreateContext(createServiceBusClient, getConnectionString, _console);

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
                _console.Error(e.Message);
                _console.Verbose(e.ToString());

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
            CreateServiceBusClient createServiceBusClient,
            GetConnectionString getConnectionString,
            SebaConsole console)
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