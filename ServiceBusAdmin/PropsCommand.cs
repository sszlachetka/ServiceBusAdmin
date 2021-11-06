using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin
{
    public class PropsCommand : SebaCommand
    {
        public PropsCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
        }

        protected override async Task<SebaResult> Execute(CancellationToken cancellationToken)
        {
            var client = CreateServiceBusClient();
            var name = await client.GetNamespaceName(cancellationToken);

            Output.WriteLine($"Namespace\t{name}");

            return SebaResult.Success;
        }
    }
}