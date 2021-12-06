using System.Linq;
using System.Threading.Tasks;
using ServiceBusAdmin.CommandHandlers.Models;

namespace ServiceBusAdmin.Tool.Subscription.Receive
{
    public class ReceivedMessageHandlingPolicy
    {
        private readonly SebaConsole _console;
        private readonly long[] _handleSequenceNumbers;
        private readonly bool _abandonUnhandledMessages;

        public ReceivedMessageHandlingPolicy(
            SebaConsole console,
            long[] handleSequenceNumbers,
            bool abandonUnhandledMessages)
        {
            _console = console;
            _handleSequenceNumbers = handleSequenceNumbers;
            _abandonUnhandledMessages = abandonUnhandledMessages;
        }

        public async Task<bool> CanHandle(IReceivedMessage message)
        {
            var canHandle = _handleSequenceNumbers.Length == 0 ||
                   _handleSequenceNumbers.Contains(message.Metadata.SequenceNumber);

            if (!canHandle) _console.Verbose($"Skip {message.Metadata.SequenceNumber}");

            if (!canHandle && _abandonUnhandledMessages)
            {
                await message.Abandon();
            }

            return canHandle;
        }
    }
}