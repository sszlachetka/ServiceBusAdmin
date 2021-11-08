using Azure.Messaging.ServiceBus;

namespace ServiceBusAdmin.Client
{
    public class TopicReceiverOptions
    {
        public TopicReceiverOptions(string topic, string subscription, ServiceBusReceiveMode receiveMode, int top)
        {
            Topic = topic;
            Subscription = subscription;
            ReceiveMode = receiveMode;
            Top = top;
        }

        public string Topic { get; }
        public string Subscription { get; }
        public ServiceBusReceiveMode ReceiveMode { get; }
        public int Top { get; }
    }
}