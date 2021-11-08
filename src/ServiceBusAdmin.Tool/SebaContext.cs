using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.Client;

namespace ServiceBusAdmin.Tool
{
    public delegate IServiceBusClient CreateServiceBusClient();

    public class SebaContext
    {
        public CreateServiceBusClient CreateServiceBusClient { get; }
        public IConsole Console { get; }

        public SebaContext(CreateServiceBusClient createServiceBusClient, IConsole console)
        {
            CreateServiceBusClient = createServiceBusClient;
            Console = console;
        }
    }
}