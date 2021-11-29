using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceBusAdmin.CommandHandlers.SendBatch;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests.Topic
{
    public class SendBatchTests : SebaCommandTests
    {
        [Fact]
        public async Task Sends_provided_messages_and_prints_them_to_console()
        {
            var enumerable = new TestAsyncEnumerable<SendMessageModel>(new[]
            {
                new SendMessageModelBuilder().WithMessageId("message1").WithBody("{\"key1\":13}").Build(),
                new SendMessageModelBuilder().WithMessageId("message2").WithBody("{\"key1\":21}").Build()
            });
            Mediator.Setup<ReadMessages, IAsyncEnumerable<SendMessageModel>>(enumerable);
            Mediator.Setup<SendBatchMessages>(
                request => request.QueueOrTopicName == "topic69",
                async request =>
                {
                    var enumerator = request.MessageEnumerator;
                    var messagesSentCallback = request.MessagesSentCallback;
                    while (await enumerator.MoveNextAsync())
                    {
                        await messagesSentCallback(new [] {enumerator.Current});
                    }
                });

            var result = await Seba().Execute(new[] {"topic", "send-batch", "topic69", "-i", "someFile"});

            AssertSuccess(result);
            AssertConsoleOutputContainJsonSubtrees(
                "{\"body\":{\"key1\":13},\"metadata\":{\"messageId\":\"message1\"}}",
                "{\"body\":{\"key1\":21},\"metadata\":{\"messageId\":\"message2\"}}");
        }
    }
}