using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using ServiceBusAdmin.CommandHandlers;
using ServiceBusAdmin.CommandHandlers.Models;
using ServiceBusAdmin.CommandHandlers.Send;
using ServiceBusAdmin.CommandHandlers.Subscription.Receive;
using ServiceBusAdmin.Tool.Subscription.Inputs;

namespace ServiceBusAdmin.Tool.Subscription.Receive
{
    public class SendToTopicCommand : SebaCommand
    {
        private readonly SubscriptionReceiverInput _subscriptionReceiverInput;

        public SendToTopicCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Name = "send-to-topic";
            Command.Description = "Receive messages from specified subscription and send them back to the topic.";
            _subscriptionReceiverInput = new SubscriptionReceiverInput(Command);
        }
        
        protected override async Task Execute(CancellationToken cancellationToken)
        {
            var options = _subscriptionReceiverInput.CreateReceiverOptions();
            var handler = new SendToTopicMessageHandler(options.EntityName.TopicName(), Console, Mediator);
            var receiveMessages = new ReceiveMessages(options, handler.Handle);
            
            await Mediator.Send(receiveMessages, cancellationToken);
        }

        private class SendToTopicMessageHandler
        {
            private readonly string _topicName;
            private readonly SebaConsole _console;
            private readonly IMediator _mediator;

            public SendToTopicMessageHandler(string topicName, SebaConsole console, IMediator mediator)
            {
                _topicName = topicName;
                _console = console;
                _mediator = mediator;
            }

            public async Task Handle(IReceivedMessage receivedMessage)
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