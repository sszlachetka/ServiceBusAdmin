using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.Client;
using ServiceBusAdmin.Tool.Subscription.Inputs;

namespace ServiceBusAdmin.Tool.Subscription.Receive
{
    public class SendToTopicCommand : SebaCommand
    {
        private readonly SubscriptionReceiverInput _subscriptionReceiverInput;

        public SendToTopicCommand(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Name = "send-to-topic";
            Command.Description = "Receives messages from specified subscription and sends them back to the topic.";
            _subscriptionReceiverInput = new SubscriptionReceiverInput(Command);
        }
        
        protected override async Task Execute(CancellationToken cancellationToken)
        {
            var options = _subscriptionReceiverInput.CreateReceiverOptions();
            var handler = new SendToTopicMessageHandler(options.EntityName.TopicName(), Console, Client);

            await Client.Receive(options, handler.Handle);
        }

        private class SendToTopicMessageHandler
        {
            private readonly string _topicName;
            private readonly SebaConsole _console;
            private readonly IServiceBusClient _client;

            public SendToTopicMessageHandler(string topicName, SebaConsole console, IServiceBusClient client)
            {
                _topicName = topicName;
                _console = console;
                _client = client;
            }

            public async Task Handle(IReceivedMessage message)
            {
                await _client.SendMessage(_topicName, message.Body, CancellationToken.None);
                await message.Complete();
                _console.Info(message.SequenceNumber);
            }
        }
    }
}