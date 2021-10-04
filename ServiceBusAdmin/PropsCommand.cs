using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin
{
    [Command]
    public class PropsCommand : SbaCommandBase
    {
        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            var admin = AdministrationClient(app);
            var response = await admin.GetNamespacePropertiesAsync();
            var props = response.Value;

            Console.WriteLine($"Namespace\t{props.Name}");

            return 0;
        }
    }
}