using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace ServiceBusAdmin.Subscription
{
    [Command("peek-dlq")]
    public class PeekDlqCommand : SubscriptionCommandBase
    {
        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            if (string.IsNullOrWhiteSpace(FullSubscriptionName)) return await base.OnExecute(app);

            Console.WriteLine($"Peek from {FullSubscriptionName}");

            return 0;
        }
    }
}