using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace AzureServiceBusExplorer.Topic
{
    [Command(Description = "Lists all topics")]
    public class ListCommand: AsbeCommandBase
    {
        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            var admin = AdministrationClient(app);
            var topics = admin.GetTopicsAsync();
            await foreach (var topic in topics)
            {
                Console.WriteLine(topic.Name);
            }

            return 0;
        }
    }
}