using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Tool
{
    public class PropsCommand : SebaCommand
    {
        public PropsCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
        }

        protected override async Task<SebaResult> Execute(CancellationToken cancellationToken)
        {
            var client = CreateClient();
            var name = await client.GetNamespaceName(cancellationToken);

            Console.WriteLine($"Namespace\t{name}");

            return SebaResult.Success;
        }
    }
}