using McMaster.Extensions.CommandLineUtils;
using MediatR;

namespace ServiceBusAdmin.Tool
{
    public delegate CommandLineApplication CreateCommand();

    public class SebaContext
    {
        public CreateCommand CreateCommand { get; }
        public SebaConsole Console { get; }
        public IMediator Mediator { get; }

        public SebaContext(CreateCommand createCommand, SebaConsole console, IMediator mediator)
        {
            CreateCommand = createCommand;
            Console = console;
            Mediator = mediator;
        }
    }
}