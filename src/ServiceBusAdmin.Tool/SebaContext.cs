using System;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.Client;

namespace ServiceBusAdmin.Tool
{
    public class SebaContext
    {
        private readonly Func<IServiceBusClient> _createServiceBusClient;
        private IServiceBusClient? _client;

        public IServiceBusClient Client => _client ??= _createServiceBusClient();
        public SebaConsole Console { get; }

        public SebaContext(Func<IServiceBusClient> createServiceBusClient, SebaConsole console)
        {
            _createServiceBusClient = createServiceBusClient;
            Console = console;
        }
    }
}