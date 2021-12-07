using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceBusAdmin.CommandHandlers.Models;

namespace ServiceBusAdmin.Tool.Subscription.Receive
{
    internal class ValidateUniqueSequenceNumberDecorator
    {
        private readonly HashSet<long> _receivedSequenceNumbers = new();
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
            var added = _receivedSequenceNumbers.Add(sequenceNumber);
            if (!added)
            {
                throw new ApplicationException($"Message with sequence number {sequenceNumber} was received more than once. Message peek-lock was released and the message again became available for receive operation. Please try processing less messages at once or settling messages with lower sequence numbers first. In general, total execution time of receive command cannot exceed lock duration configured for given Service Bus entity.");
            }
        }
    }
}