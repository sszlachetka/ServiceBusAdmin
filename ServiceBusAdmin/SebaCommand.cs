using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.ServiceBusClient;

namespace ServiceBusAdmin
{
    public abstract class SebaCommand
    {
        public CommandLineApplication Command { get; }
        protected SebaContext Context { get; }

        protected SebaCommand(SebaContext context, CommandLineApplication parentCommand)
        {
            Context = context;
            Command = new CommandLineApplication();
            Command.Parent = parentCommand;
            Command.Name = GetType().Name.ToLower().Replace("command", string.Empty);
            Command.OnExecuteAsync(async cancellationToken => (int) await Execute(cancellationToken));
        }

        protected virtual Task<SebaResult> Execute(CancellationToken cancellationToken)
        {
            Command.ShowHelp();
            return Task.FromResult(SebaResult.Failure);
        }

        protected IServiceBusClient CreateClient()
        {
            return Context.CreateServiceBusClient();
        }

        protected IOutputWriter Output => Context.OutputWriter;
    }
}