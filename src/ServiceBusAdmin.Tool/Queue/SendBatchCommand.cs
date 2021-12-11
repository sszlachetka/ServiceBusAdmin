using System;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.Tool.Queue.Arguments;
using ServiceBusAdmin.Tool.SendBatch;

namespace ServiceBusAdmin.Tool.Queue
{
    public class SendBatchCommand : SendBatchCommandBase
    {
        private readonly Func<string> _getQueueName;

        public SendBatchCommand(SebaContext context, CommandLineApplication parentCommand) : base(context,
            parentCommand)
        {
            Command.Description = "Send batch of messages to a queue.";
            _getQueueName = Command.ConfigureQueueNameArgument();
        }

        protected override string GetEntityName()
        {
            return _getQueueName();
        }
    }
}