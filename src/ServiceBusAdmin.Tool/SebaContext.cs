using McMaster.Extensions.CommandLineUtils;
using MediatR;
using ServiceBusAdmin.Client;

namespace ServiceBusAdmin.Tool
{
    public delegate CommandLineApplication CreateCommand();
    public delegate IServiceBusClient CreateServiceBusClient();

    public class SebaContext
    {
        private readonly CreateServiceBusClient _createServiceBusClient;
        private IServiceBusClient? _client;

        public IServiceBusClient Client => _client ??= _createServiceBusClient();
        public CreateCommand CreateCommand { get; }
        public SebaConsole Console { get; }
        public IMediator Mediator { get; }

        public SebaContext(CreateServiceBusClient createServiceBusClient, CreateCommand createCommand, SebaConsole console, IMediator mediator)
        {
            _createServiceBusClient = createServiceBusClient;
            CreateCommand = createCommand;
            Console = console;
            Mediator = mediator;
        }
    }
}