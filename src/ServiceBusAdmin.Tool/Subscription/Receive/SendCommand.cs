using System;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using ServiceBusAdmin.CommandHandlers;
using ServiceBusAdmin.CommandHandlers.Models;
using ServiceBusAdmin.CommandHandlers.Send;
using ServiceBusAdmin.CommandHandlers.Subscription.Receive;
using ServiceBusAdmin.Tool.Subscription.Inputs;
using ServiceBusAdmin.Tool.Subscription.Receive.Options;

namespace ServiceBusAdmin.Tool.Subscription.Receive
{
    public class SendCommand : SebaCommand
    {
        private readonly Func<long[]> _handleSequenceNumbers;
        private readonly SubscriptionReceiverInput _subscriptionReceiverInput;

        public SendCommand(SebaContext context,
            CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Description = "Receive messages from given subscription and send them back to the topic.";
            _handleSequenceNumbers = Command.ConfigureHandleSequenceNumbers();
            _subscriptionReceiverInput = new SubscriptionReceiverInput(Command);
        }
        
        protected override async Task Execute(CancellationToken cancellationToken)
        {
            var options = _subscriptionReceiverInput.CreateReceiverOptions();
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
            return new SendMessageCallback(
                options.EntityName.TopicName(),
                Console,
                Mediator);
        }

        private class SendMessageCallback
        {
            private readonly string _topicName;
            private readonly SebaConsole _console;
            private readonly IMediator _mediator;

            public SendMessageCallback(
                string topicName,
                SebaConsole console,
                IMediator mediator)
            {
                _topicName = topicName;
                _console = console;
                _mediator = mediator;
            }

            public async Task Callback(IReceivedMessage receivedMessage)
            {
                var messageMetadata = receivedMessage.Metadata.ConvertToSendMessage();
                var message = new SendMessageModel(receivedMessage.Body, messageMetadata);
                var sendMessage = new SendMessage(_topicName, message);

                await _mediator.Send(sendMessage);
                await receivedMessage.Complete();

                _console.Info(receivedMessage.Metadata.SequenceNumber);
            }
        }
    }
}