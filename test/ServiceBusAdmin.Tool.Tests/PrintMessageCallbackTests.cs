using System;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Json;
using Newtonsoft.Json.Linq;
using ServiceBusAdmin.CommandHandlers.Models;
using ServiceBusAdmin.Tool.Subscription.Options;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests
{
    public class PrintMessageCallbackTests
    {
        private readonly TestConsole _console;
        
        public PrintMessageCallbackTests()
        {
            _console = new TestConsole();
        }

        [Fact]
        public async Task Prints_metadata_when_requested()
        {
            var date = new DateTimeOffset(2010, 1, 2, 3, 4, 5, TimeSpan.Zero);
            var print = CreateCallback(OutputContentEnum.Metadata);
            var message = new TestMessageBuilder()
                .WithSequenceNumber(26)
                .WithMessageId("someId")
                .WithEnqueuedTime(date.AddDays(3))
                .WithExpiresAt(date.AddDays(5))
                .WithApplicationProperty("key1", 13)
                .WithApplicationProperty("key2", "someValue")
                .Build();

            await print.Callback(message);

            AssertConsoleOutputContainsJsonSubtree(
                "\"sequenceNumber\":26",
                "\"messageId\":\"someId\"",
                "\"applicationProperties\":{\"key1\":13,\"key2\":\"someValue\"}"
            );
            AssertConsoleOutputHasJsonElementWithDateTimeOffset("enqueuedTime").Should().Be(date.AddDays(3));
            AssertConsoleOutputHasJsonElementWithDateTimeOffset("expiresAt").Should().Be(date.AddDays(5));
        }

        private void AssertConsoleOutputContainsJsonSubtree(params string[] elements)
        {
            var json = $"{{{string.Join(",", elements)}}}";

            JToken.Parse(_console.OutputText).Should().ContainSubtree(JToken.Parse(json));
        }

        private DateTimeOffset AssertConsoleOutputHasJsonElementWithDateTimeOffset(string elementName)
        {
            var subject = JToken.Parse(_console.OutputText).Should().HaveElement(elementName).Subject;

            return DateTimeOffset.Parse(subject.Value<string>());
        }

        private PrintMessageCallback CreateCallback(OutputContentEnum outputContent)
        {
            return new PrintMessageCallback(MessageBodyFormatEnum.Json, OutputContentEnum.Metadata, Encoding.UTF8,
                new SebaConsole(_console, isVerboseOutput: () => false));
        }
    }
}