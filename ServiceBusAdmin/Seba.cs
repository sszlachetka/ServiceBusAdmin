using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.Subscription;
using ServiceBusAdmin.Topic;

namespace ServiceBusAdmin
{
    public delegate IServiceBusClient CreateServiceBusClientWith(string connectionString);

    public class Seba
    {
        public static async Task<int> Main(string[] args)
        {
            var seba = new Seba(
                connectionString => new SebaClient(connectionString),
                new OutputWriter(),
                Environment.GetEnvironmentVariable);

            return (int) await seba.Execute(args);
        }

        private readonly CommandLineApplication _app;
        private readonly IOutputWriter _outputWriter;

        public Seba(
            CreateServiceBusClientWith createServiceBusClient,
            IOutputWriter outputWriter,
            GetEnvironmentVariable getEnvironmentVariable)
        {
            _outputWriter = outputWriter;
            _app = CreateApplication();
            var getConnectionString = _app.ConfigureConnectionStringOption(getEnvironmentVariable);
            var context = CreateContext(createServiceBusClient, outputWriter, getConnectionString);
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
                _outputWriter.WriteErrorLine(e.Message);

                return SebaResult.Failure;
            }
        }

        private static CommandLineApplication CreateApplication()
        {
            var app = new CommandLineApplication
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
            IOutputWriter outputWriter,
            GetConnectionString getConnectionString)
        {
            IServiceBusClient CreateServiceBusClient() => createServiceBusClient(getConnectionString());

            return new SebaContext(CreateServiceBusClient, outputWriter);
        }

        private static void ConfigureCommands(CommandLineApplication app, SebaContext context)
        {
            app.Configure(new PropsCommand(context));
            app.Configure(new TopicCommand(context));
            app.Configure(new SubscriptionCommand(context));
        }
    }
}