using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using ServiceBusAdmin.CommandHandlers.FilesAccess;
using ServiceBusAdmin.CommandHandlers.SendBatch;
using ServiceBusAdmin.Tool.MessageParsing;
using ServiceBusAdmin.Tool.Options;

namespace ServiceBusAdmin.Tool.SendBatch
{
    public abstract class SendBatchCommandBase: SebaCommand
    {
        private readonly Func<string> _getInputFile;
        private readonly Func<Encoding> _getInputFileEncoding;
        private readonly Func<Encoding> _getSendMessageEncoding;

        protected SendBatchCommandBase(SebaContext context, CommandLineApplication parentCommand) : base(context, parentCommand)
        {
            Command.Name = "send-batch";
            _getInputFile = Command.ConfigureInputFileOption();
            _getInputFileEncoding = Command.ConfigureEncodingNameOption("Input file encoding", "--input-file-encoding");
            _getSendMessageEncoding = Command.ConfigureEncodingNameOption("Send message encoding", "--send-message-encoding");
        }

        protected override async Task Execute(CancellationToken cancellationToken)
        {
            await using var fileStream = await Mediator.Send(new ReadFile(_getInputFile()), cancellationToken);
            var enumerator = new SendMessageEnumerator(new StreamReader(fileStream, _getInputFileEncoding()),
                new SendMessageParser(_getSendMessageEncoding()));
            var print = new PrintSentMessagesCallback(Console, _getSendMessageEncoding());

            var sendBatchMessages = new SendBatchMessages(GetEntityName(), enumerator, print.Callback);

            await Mediator.Send(sendBatchMessages, cancellationToken);
        }

        protected abstract string GetEntityName();
    }
}