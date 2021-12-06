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
    public class SendToTopicCommand : SebaCommand
    {
        private readonly ReceiveOptions _receiveOptions;
        private readonly SubscriptionReceiverInput _subscriptionReceiverInput;

        public SendToTopicCommand(SebaContext context,
            CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Name = "send-to-topic";
            Command.Description = "Receive messages from specified subscription and send them back to the topic.";
            _receiveOptions = Command.ConfigureReceiveOptions();
            _subscriptionReceiverInput = new SubscriptionReceiverInput(Command);
        }
        
        protected override async Task Execute(CancellationToken cancellationToken)
        {
            var options = _subscriptionReceiverInput.CreateReceiverOptions();
            var sendToTopic = CreateSendToTopicCallback(options);
            var receiveMessages = new ReceiveMessages(options, sendToTopic.Callback);
            
            await Mediator.Send(receiveMessages, cancellationToken);
        }

        private SendToTopicMessageCallback CreateSendToTopicCallback(ReceiverOptions options)
        {
            return new SendToTopicMessageCallback(
                options.EntityName.TopicName(),
                Console,
                Mediator,
                _receiveOptions.CreateMessageHandlingPolicy(Console));
        }

        private class SendToTopicMessageCallback
        {
            private readonly string _topicName;
            private readonly SebaConsole _console;
            private readonly IMediator _mediator;
            private readonly ReceivedMessageHandlingPolicy _handlingPolicy;

            public SendToTopicMessageCallback(
                string topicName,
                SebaConsole console,
                IMediator mediator,
                ReceivedMessageHandlingPolicy handlingPolicy)
            {
                _topicName = topicName;
                _console = console;
                _mediator = mediator;
                _handlingPolicy = handlingPolicy;
            }

            public async Task Callback(IReceivedMessage receivedMessage)
            {
                if (!await _handlingPolicy.CanHandle(receivedMessage))
                {
                    return;
                }
                
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