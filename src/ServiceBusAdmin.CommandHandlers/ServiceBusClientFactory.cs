using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

namespace ServiceBusAdmin.CommandHandlers
{
    internal class ServiceBusClientFactory
    {
        private readonly GetServiceBusConnectionString _getConnectionString;

        public ServiceBusClientFactory(GetServiceBusConnectionString getConnectionString)
        {
            _getConnectionString = getConnectionString;
        }
        
        public ServiceBusAdministrationClient AdministrationClient()
        {
            return new (_getConnectionString());
        }

        public ServiceBusClient ServiceBusClient()
        {
            return new(_getConnectionString());
        }
    }
}