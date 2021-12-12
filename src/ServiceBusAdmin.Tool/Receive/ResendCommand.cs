using System;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using ServiceBusAdmin.CommandHandlers;
using ServiceBusAdmin.CommandHandlers.Models;
using ServiceBusAdmin.CommandHandlers.Receive;
using ServiceBusAdmin.CommandHandlers.Send;
using ServiceBusAdmin.Tool.Input;
using ServiceBusAdmin.Tool.Options;

namespace ServiceBusAdmin.Tool.Receive
{
    public class ResendCommand : SebaCommand
    {
        private readonly Func<long[]> _handleSequenceNumbers;
        private readonly ReceiverInput _receiverInput;

        public ResendCommand(SebaContext context,
            CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Description = $"Receive messages from given entity and send them back to the entity of origin. Main use case of this command is to receive messages from DLQ (use {IsDeadLetterSubQueue.Template} option for this purpose) and send them back to the queue or topic.";
            _handleSequenceNumbers = Command.ConfigureHandleSequenceNumbers();
            _receiverInput = new ReceiverInput(Command);
        }
        
        protected override async Task Execute(CancellationToken cancellationToken)
        {
            var options = _receiverInput.CreateReceiverOptions();
            var sendToTopic = CreateSendToTopicCallback(options);
            var handleSequenceNumbersDecorator =
                new HandleSequenceNumbersDecorator(Console, _handleSequenceNumbers(), sendToTopic.Callback);
            var validateDecorator = new ValidateUniqueSequenceNumberDecorator(handleSequenceNumbersDecorator.Callback);

            var receiveMessages = new ReceiveMessages(options, validateDecorator.Callback);
            
            await Mediator.Send(receiveMessages, cancellationToken);
            
            validateDecorator.VerifyAllReceived(_handleSequenceNumbers());
        }

        private SendMessageCallback CreateSendToTopicCallback(ReceiverOptions options)
        {
            var entityName = options.EntityName;
            
            return new SendMessageCallback(
                entityName.IsQueue ? entityName.QueueName() : entityName.TopicName(),
                Console,
                Mediator);
        }

        private class SendMessageCallback
        {
            private readonly string _queueOrTopicName;
            private readonly SebaConsole _console;
            private readonly IMediator _mediator;

            public SendMessageCallback(
                string queueOrTopicName,
                SebaConsole console,
                IMediator mediator)
            {
                _queueOrTopicName = queueOrTopicName;
                _console = console;
                _mediator = mediator;
            }

            public async Task Callback(IReceivedMessage receivedMessage)
            {
                var messageMetadata = receivedMessage.Metadata.ConvertToSendMessage();
                var message = new SendMessageModel(receivedMessage.Body, messageMetadata);
                var sendMessage = new SendMessage(_queueOrTopicName, message);

                await _mediator.Send(sendMessage);
                await receivedMessage.Complete();

                _console.Info(receivedMessage.Metadata.SequenceNumber);
            }
        }
    }
}