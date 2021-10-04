using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Subscription
{
    [Command]
    public class PropsCommand : SubscriptionCommandBase
    {
        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            var admin = AdministrationClient(app);
            var (topic, subscription) = ParseFullSubscriptionName();
            var response = await admin.GetSubscriptionRuntimePropertiesAsync(topic, subscription);
            var props = response.Value;

            Console.WriteLine($"ActiveMessageCount\t{props.ActiveMessageCount}");
            Console.WriteLine($"DeadLetterMessageCount\t{props.DeadLetterMessageCount}");

            return 0;
        }
    }
}