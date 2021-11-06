using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin
{
    public class PropsCommand : SebaCommand
    {
        public PropsCommand(SebaContext context) : base(context)
        {
        }
        
        protected override async Task ExecuteAsync(CommandLineApplication command, CancellationToken cancellationToken)
        {
            var client = CreateServiceBusClient();
            var name = await client.GetNamespaceName(cancellationToken);

            Output.WriteLine($"Namespace\t{name}");
        }
    }
}