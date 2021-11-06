using ServiceBusAdmin.ServiceBusClient;

namespace ServiceBusAdmin
{
    public delegate IServiceBusClient CreateServiceBusClient();

    public class SebaContext
    {
        public CreateServiceBusClient CreateServiceBusClient { get; }
        public IOutputWriter OutputWriter { get; }

        public SebaContext(CreateServiceBusClient createServiceBusClient, IOutputWriter outputWriter)
        {
            CreateServiceBusClient = createServiceBusClient;
            OutputWriter = outputWriter;
        }
    }
}