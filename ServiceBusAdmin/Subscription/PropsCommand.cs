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
            if (string.IsNullOrWhiteSpace(FullSubscriptionName)) return await base.OnExecute(app);

            var admin = AdministrationClient(app);
            var (topic, subscription) = ParseFullSubscriptionName();
            var response = await admin.GetSubscriptionRuntimePropertiesAsync(topic, subscription);
            var runtimeProperties = response.Value;

            Console.WriteLine($"ActiveMessageCount\t{runtimeProperties.ActiveMessageCount}");
            Console.WriteLine($"DeadLetterMessageCount\t{runtimeProperties.DeadLetterMessageCount}");

            return 0;
        }
    }
}