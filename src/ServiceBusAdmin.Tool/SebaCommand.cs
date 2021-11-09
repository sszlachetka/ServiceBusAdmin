using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.Client;

namespace ServiceBusAdmin.Tool
{
    public abstract class SebaCommand
    {
        private const string CommandSuffix = "command";
        public CommandLineApplication Command { get; }
        protected SebaContext Context { get; }

        protected SebaCommand(SebaContext context, CommandLineApplication parentCommand)
        {
            Context = context;
            Command = new CommandLineApplication(context.Console.InternalConsole);
            Command.Parent = parentCommand;
            Command.Name = GetType().Name.ToLower().Replace(CommandSuffix, string.Empty);
            Command.OnExecuteAsync(async cancellationToken => (int) await ExecuteInternal(cancellationToken));
        }
        
        private async Task<SebaResult> ExecuteInternal(CancellationToken cancellationToken)
        {
            await Execute(cancellationToken);
            return SebaResult.Success;
        }

        protected virtual Task Execute(CancellationToken cancellationToken)
        {
            Command.ShowHelp();
            return Task.CompletedTask;
        }

        protected IServiceBusClient Client => Context.Client;

        protected SebaConsole Console => Context.Console;
    }
}