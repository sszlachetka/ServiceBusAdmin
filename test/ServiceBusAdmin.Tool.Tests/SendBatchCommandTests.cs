using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ServiceBusAdmin.CommandHandlers.FilesAccess;
using ServiceBusAdmin.CommandHandlers.SendBatch;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests
{
    public class SendBatchCommandTests : SebaCommandTests
    {
        [Theory]
        [InlineData("queue")]
        [InlineData("topic")]
        public async Task Sends_provided_messages_and_prints_them_to_console(string parentCommand)
        {
            GivenReadFileReturns(
                "{\"metadata\":{\"messageId\":\"M1\"},\"body\":{\"key1\":13}}",
                "{\"metadata\":{\"messageId\":\"M2\"},\"body\":\"<some><xml>value</xml></some>\"}");
            GivenSendBatchHandlesEntity("entity69");

            var result = await Seba().Execute(new[] {parentCommand, "send-batch", "entity69", "-i", "someFile"});

            AssertSuccess(result);
            AssertConsoleOutputContainJsonSubtrees(
                "{\"body\":{\"key1\":13},\"metadata\":{\"messageId\":\"M1\"}}",
                "{\"body\":\"<some><xml>value</xml></some>\",\"metadata\":{\"messageId\":\"M2\"}}");
        }

        [Theory]
        [InlineData("queue", "Queue")]
        [InlineData("topic", "Topic")]
        public async Task Entity_argument_is_required(string parentCommand, string argumentName)
        {
            var result = await Seba().Execute(new[] { parentCommand, "send-batch", "-i", "someFile" });

            AssertFailure(result, $"The {argumentName} name field is required.");
        }
        
        [Theory]
        [InlineData("queue")]
        [InlineData("topic")]
        public async Task Input_file_option_is_required(string parentCommand)
        {
            var result = await Seba().Execute(new[] {parentCommand, "send-batch", "entity69"});

            AssertFailure(result, "The --input-file field is required.");
        }

        private void GivenReadFileReturns(params string[] lines)
        {
            var fileContent = string.Join(Environment.NewLine, lines);
            var fileStream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));
            Mediator.Setup<ReadFile, Stream>(fileStream);
        }

        private void GivenSendBatchHandlesEntity(string entityName)
        {
            Mediator.Setup<SendBatchMessages>(
                request => request.QueueOrTopicName == entityName,
                SendBatchHandler);
        }

        private static async Task SendBatchHandler(SendBatchMessages request)
        {
            var (_, enumerator, messagesSentCallback) = request;
            while (await enumerator.MoveNextAsync())
            {
                await messagesSentCallback(new[] { enumerator.Current });
            }
        }
    }
}