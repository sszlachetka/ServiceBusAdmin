using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Subscription.Peek
{
    internal class PeekMessagesHandler : IRequestHandler<PeekMessages>
    {
        private readonly ServiceBusClientFactory _clientFactory;

        public PeekMessagesHandler(ServiceBusClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<Unit> Handle(PeekMessages request, CancellationToken cancellationToken)
        {
            var options = request.Options;
            await using var client = _clientFactory.ServiceBusClient();
            await using var receiver = ServiceBusReceiver(client, options);

            var peekedCount = 0;
            IReadOnlyList<ServiceBusReceivedMessage> messages;
            do
            {
                messages = await receiver.PeekMessagesAsync(options.MaxMessages - peekedCount, null, cancellationToken);
                peekedCount += messages.Count;
                var peekedMessages = messages.Select(m => new MessageAdapter(m)).ToList();
                await HandlePeekedMessages(peekedMessages, request.Handler, options.MessageHandlingConcurrencyLevel);
            } while (messages.Count > 0 && peekedCount < options.MaxMessages);

            return Unit.Value;
        }

        private static async Task HandlePeekedMessages(
            IReadOnlyCollection<IMessage> messages,
            MessageHandler messageHandler,
            int messageHandlingConcurrencyLevel)
        {
            if (messages.Count == 0) return;

            var semaphore = new SemaphoreSlim(messageHandlingConcurrencyLevel);
            var tasks = new List<Task>();
            foreach (var message in messages)
            {
                await semaphore.WaitAsync();

                tasks.Add(messageHandler(message)
                    .ContinueWith(_ => semaphore.Release()));
            }
            
            await Task.WhenAll(tasks);
        }
        
        private static ServiceBusReceiver ServiceBusReceiver(ServiceBusClient client, ReceiverOptions options)
        {
            var entity = options.EntityName;
            var serviceBusReceiverOptions = new ServiceBusReceiverOptions
            {
                SubQueue = options.IsDeadLetterSubQueue ? SubQueue.DeadLetter : SubQueue.None
            };

            return client.CreateReceiver(entity.TopicName(), entity.SubscriptionName(), serviceBusReceiverOptions);
        }
    }
}