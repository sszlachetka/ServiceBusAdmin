using MediatR;

namespace ServiceBusAdmin.CommandHandlers.Subscription.Peek
{
    public record PeekMessages : IRequest
    {
        public PeekMessages(ReceiverOptions options, MessageHandler handler)
        {
            Options = options;
            Handler = handler;
        }

        public ReceiverOptions Options { get; }
        public MessageHandler Handler { get; }
    }
}