using System;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin
{
    public abstract class SebaCommand
    {
        protected readonly SebaContext Context;
        public virtual string Name => GetType().Name.ToLower().Replace("command", string.Empty);
        protected virtual string? Description => null;

        protected SebaCommand(SebaContext context)
        {
            Context = context;
        }

        public void Configure(CommandLineApplication command)
        {
            command.Description = Description;
            command.OnExecuteAsync(UseExecuteAsync(command));
            ConfigureSubCommands(command);
            ConfigureArgsAndOptions(command);
        }

        protected virtual void ConfigureArgsAndOptions(CommandLineApplication command)
        {
        }

        protected virtual void ConfigureSubCommands(CommandLineApplication command)
        {
        }

        private Func<CancellationToken, Task<int>> UseExecuteAsync(CommandLineApplication command)
        {
            return async cancellationToken =>
            {
                await ExecuteAsync(command, cancellationToken);
                return (int) SebaResult.Success;
            };
        }

        protected virtual Task ExecuteAsync(CommandLineApplication command, CancellationToken cancellationToken)
        {
            command.ShowHelp();
            return Task.CompletedTask;
        }

        protected IServiceBusClient CreateServiceBusClient()
        {
            return Context.CreateServiceBusClient();
        }

        protected IOutputWriter Output => Context.OutputWriter;
    }
}