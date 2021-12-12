using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using ServiceBusAdmin.CommandHandlers.Models;

namespace ServiceBusAdmin.Tool.Receive
{
    internal class ValidateUniqueSequenceNumberDecorator
    {
        private readonly ConcurrentDictionary<long, bool> _receivedSequenceNumbers = new();
        private readonly Func<IReceivedMessage, Task> _innerCallback;

        public ValidateUniqueSequenceNumberDecorator(Func<IReceivedMessage, Task> innerCallback)
        {
            _innerCallback = innerCallback;
        }

        public Task Callback(IReceivedMessage message)
        {
            Validate(message);

            return _innerCallback(message);
        }
        
        private void Validate(IReceivedMessage message)
        {
            var sequenceNumber = message.Metadata.SequenceNumber;
            var added = _receivedSequenceNumbers.TryAdd(sequenceNumber, true);
            if (!added)
            {
                throw new ApplicationException($"Message with sequence number {sequenceNumber} was received more than once. Message peek-lock was released and the message again became available for receive operation. Please try processing less messages at once or settling messages with lower sequence numbers first. In general, total execution time of receive command cannot exceed lock duration configured for given Service Bus entity.");
            }
        }

        public void VerifyAllReceived(long[] expectedSequenceNumbers)
        {
            if (expectedSequenceNumbers.Length == 0) return;

            var notReceived = expectedSequenceNumbers.Except(_receivedSequenceNumbers.Keys).ToArray();
            if (notReceived.Length == 0) return;

            throw new ApplicationException("Following sequence numbers were not received: " +
                                           string.Join(", ", notReceived));
        }
    }
}