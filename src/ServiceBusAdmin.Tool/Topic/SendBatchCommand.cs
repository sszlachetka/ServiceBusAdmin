using System;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.Tool.Arguments;
using ServiceBusAdmin.Tool.SendBatch;

namespace ServiceBusAdmin.Tool.Topic
{
    public class SendBatchCommand : SendBatchCommandBase
    {
        private readonly Func<string> _getTopicName;

        public SendBatchCommand(SebaContext context, CommandLineApplication parentCommand) : base(context,
            parentCommand)
        {
            Command.Description = "Send batch of messages to a topic.";
            _getTopicName = Command.ConfigureTopicNameArgument();
        }

        protected override string GetEntityName()
        {
            return _getTopicName();
        }
    }
}