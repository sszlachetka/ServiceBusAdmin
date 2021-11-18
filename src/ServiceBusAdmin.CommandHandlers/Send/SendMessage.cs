using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Send
{
    public record SendMessage : IRequest
    {
        public SendMessage(string queueOrTopicName, string messageBody)
        {
            QueueOrTopicName = queueOrTopicName;
            MessageBody = messageBody;
        }

        public string QueueOrTopicName { get; }
        public string MessageBody { get; }
    }
}