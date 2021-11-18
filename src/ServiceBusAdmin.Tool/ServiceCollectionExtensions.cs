using System;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using ServiceBusAdmin.Client;
using ServiceBusAdmin.CommandHandlers;
using ServiceBusAdmin.Tool.Options;

namespace ServiceBusAdmin.Tool
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSeba(this IServiceCollection services, 
            IConsole console,
            GetEnvironmentVariable getEnvironmentVariable,
            Func<string, IServiceBusClient> createServiceBusClient)
        {
            services.AddSingleton(console);
            services.AddSingleton<SebaConsole>();
            services.AddCommandLineApplication(console, getEnvironmentVariable);
            services.AddSingleton<CreateServiceBusClient>(sp =>
            {
                var getConnectionString = sp.GetRequiredService<GetServiceBusConnectionString>();
                return () => createServiceBusClient(getConnectionString());
            });
            services.AddSingleton<CreateCommand>(sp =>
                () => new CommandLineApplication(sp.GetRequiredService<IConsole>()));
            services.AddSingleton<SebaContext>();
            services.AddSingleton<Seba>();
        }

        private static void AddCommandLineApplication(this IServiceCollection services, IConsole console,
            GetEnvironmentVariable getEnvironmentVariable)
        {
            var app = new CommandLineApplication(console);
            var isVerboseOutput = app.ConfigureVerboseOption();
            var getConnectionString = app.ConfigureConnectionStringOption(getEnvironmentVariable);

            services.AddSingleton(app);
            services.AddSingleton(isVerboseOutput);
            services.AddSingleton(getConnectionString);
        }
    }
}