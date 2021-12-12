using System;
using System.Linq;
using System.Threading.Tasks;
using ServiceBusAdmin.CommandHandlers.Models;

namespace ServiceBusAdmin.Tool.Receive
{
    public class HandleSequenceNumbersDecorator
    {
        private readonly SebaConsole _console;
        private readonly long[] _handleSequenceNumbers;
        private readonly Func<IReceivedMessage, Task> _innerCallback;

        public HandleSequenceNumbersDecorator(
            SebaConsole console,
            long[] handleSequenceNumbers,
            Func<IReceivedMessage, Task> innerCallback)
        {
            _console = console;
            _handleSequenceNumbers = handleSequenceNumbers;
            _innerCallback = innerCallback;
        }

        public Task Callback(IReceivedMessage message)
        {
            var canHandle = _handleSequenceNumbers.Length == 0 ||
                            _handleSequenceNumbers.Contains(message.Metadata.SequenceNumber);

            if (canHandle) return _innerCallback(message);

            _console.Verbose($"Skip {message.Metadata.SequenceNumber}");
            return Task.CompletedTask;
        }
    }
}