using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using MediatR;

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
            Command = context.CreateCommand();
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

        protected SebaConsole Console => Context.Console;

        protected IMediator Mediator => Context.Mediator;
    }
}