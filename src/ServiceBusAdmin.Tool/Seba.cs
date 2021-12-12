using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using ServiceBusAdmin.CommandHandlers;
using ServiceBusAdmin.Tool.Peek;
using ServiceBusAdmin.Tool.Queue;
using ServiceBusAdmin.Tool.Receive;
using ServiceBusAdmin.Tool.SendBatch;
using ServiceBusAdmin.Tool.Subscription;
using ServiceBusAdmin.Tool.Topic;

namespace ServiceBusAdmin.Tool
{
    public class Seba
    {
        public static async Task<int> Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddCommandHandlers();
            services.AddSeba(PhysicalConsole.Singleton, Environment.GetEnvironmentVariable);
            await using var provider = services.BuildServiceProvider();

            var seba = provider.GetRequiredService<Seba>();

            return (int) await seba.Execute(args);
        }

        private readonly CommandLineApplication _app;
        private readonly SebaConsole _console;

        public Seba(
            CommandLineApplication app,
            SebaContext context)
        {
            _app = ConfigureApp(app, context);
            _console = context.Console;
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

        private static CommandLineApplication ConfigureApp(CommandLineApplication app, SebaContext context)
        {
            app.Name = "seba";
            app.Description = "Azure Service Bus administration utility";
            app.HelpOption(inherited: true);
            app.VersionOptionFromAssemblyAttributes("-v|--version", typeof(Seba).Assembly);
            app.OnExecute(() =>
            {
                app.ShowHelp();
                return (int) SebaResult.Failure;
            });
            
            return ConfigureCommands(app, context);
        }
        
        private static CommandLineApplication ConfigureCommands(CommandLineApplication app, SebaContext context)
        {
            app.Subcommand(new PeekCommand(context, app));
            app.Subcommand(new PropsCommand(context, app));
            app.Subcommand(new QueueCommand(context, app));
            app.Subcommand(new ReceiveCommand(context, app));
            app.Subcommand(new SendCommand(context, app));
            app.Subcommand(new SendBatchCommand(context, app));
            app.Subcommand(new SubscriptionCommand(context, app));
            app.Subcommand(new TopicCommand(context, app));

            return app;
        }
    }
}