using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using MediatR;
using ServiceBusAdmin.CommandHandlers.Models;

namespace ServiceBusAdmin.CommandHandlers.SendBatch
{
    internal class SendBatchMessagesHandler : IRequestHandler<SendBatchMessages>
    {
        private readonly ServiceBusClientFactory _clientFactory;

        public SendBatchMessagesHandler(ServiceBusClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<Unit> Handle(SendBatchMessages request, CancellationToken cancellationToken)
        {
            var (queueOrTopicName, encoding, bodyFormat, messages, callback) = request;
            
            await using var client = _clientFactory.ServiceBusClient();
            await using var sender = client.CreateSender(queueOrTopicName);

            var batchFactory = new MessageBatchFactory(sender, encoding, bodyFormat);
            var readNext = await messages.MoveNextAsync();
            do
            {
                using var batch = await batchFactory.CreateBatch(cancellationToken);
                while (readNext)
                {
                    if (!batch.TryAddMessage(messages.Current)) break;

                    readNext = await messages.MoveNextAsync();
                }

                await batch.Send(cancellationToken);
                await callback(batch.Messages);
            } while (readNext);

            return Unit.Value;
        }

        private class MessageBatchFactory
        {
            private readonly ServiceBusSender _sender;
            private readonly Encoding _encoding;
            private readonly MessageBodyFormatEnum _bodyFormat;

            public MessageBatchFactory(ServiceBusSender sender, Encoding encoding, MessageBodyFormatEnum bodyFormat)
            {
                _sender = sender;
                _encoding = encoding;
                _bodyFormat = bodyFormat;
            }

            public async Task<MessageBatch> CreateBatch(CancellationToken cancellationToken)
            {
                var batch = await _sender.CreateMessageBatchAsync(cancellationToken);

                return new MessageBatch(batch, _sender, _encoding, _bodyFormat);
            }
        }

        private class MessageBatch : IDisposable
        {
            private readonly ServiceBusMessageBatch _batch;
            private readonly ServiceBusSender _sender;
            private readonly Encoding _encoding;
            private readonly MessageBodyFormatEnum _bodyFormat;
            private readonly List<SendMessageModel> _batchMessages;

            public MessageBatch(ServiceBusMessageBatch batch, ServiceBusSender sender, Encoding encoding, MessageBodyFormatEnum bodyFormat)
            {
                _batch = batch;
                _sender = sender;
                _encoding = encoding;
                _bodyFormat = bodyFormat;
                _batchMessages = new List<SendMessageModel>();
            }

            public bool TryAddMessage(SendMessageModel message)
            {
                var added = _batch.TryAddMessage(message.MapToServiceBusMessage(_encoding, _bodyFormat));
                if (added) _batchMessages.Add(message);

                return added;
            }

            public Task Send(CancellationToken cancellationToken)
            {
                return _sender.SendMessagesAsync(_batch, cancellationToken);
            }

            public IReadOnlyCollection<SendMessageModel> Messages => _batchMessages;

            public void Dispose()
            {
                _batch.Dispose();
            }
        }
    }
}