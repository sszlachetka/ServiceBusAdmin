using System;
using System.Text;
using FluentAssertions;
using FluentAssertions.Json;
using Newtonsoft.Json.Linq;
using ServiceBusAdmin.Tool.SendBatch;
using Xunit;

namespace ServiceBusAdmin.Tool.Tests.SendBatch
{
    public class SendMessageParserTests
    {
        [Fact]
        public void Can_parse_json_object_body()
        {
            const string bodyString = "{\"key1\":69,\"key2\":{\"nested\":\"value\"}}";
            var rawMessage = string.Concat("{\"body\":", bodyString, "}");
            var parser = new SendMessageParser(Encoding.UTF8);

            var message = parser.Parse(rawMessage);

            JToken.Parse(message.Body.ToString()).Should().BeEquivalentTo(JToken.Parse(bodyString));
        }
        
        [Fact]
        public void Can_parse_string_body()
        {
            const string rawMessage = "{\"body\":\"string-value\"}";
            var parser = new SendMessageParser(Encoding.UTF8);

            var message = parser.Parse(rawMessage);

            message.Body.ToString().Should().Be("string-value");
        }
        
        [Fact]
        public void Can_parse_xml_string_body()
        {
            const string rawMessage = "{\"body\":\"<some><test>value</test></some>\"}";
            var parser = new SendMessageParser(Encoding.UTF8);

            var message = parser.Parse(rawMessage);

            message.Body.ToString().Should().Be("<some><test>value</test></some>");
        }

        [Fact]
        public void Body_must_be_provided()
        {
            const string rawMessage = "{\"metadata\":{\"messageId\":\"someId\"}}";
            var parser = new SendMessageParser(Encoding.UTF8);

            Action action = () => parser.Parse(rawMessage);

            action.Should().Throw<ApplicationException>().Which.Message.Should().Be("Message body is undefined.");
        }
        
        [Fact]
        public void Body_cannot_be_null()
        {
            const string rawMessage = "{\"body\":null}";
            var parser = new SendMessageParser(Encoding.UTF8);

            Action action = () => parser.Parse(rawMessage);

            action.Should().Throw<ApplicationException>().Which.Message.Should().Be("Message body is null.");
        }

        [Theory]
        [InlineData("69", "Number")]
        [InlineData("66.6", "Number")]
        [InlineData("true", "True")]
        [InlineData("false", "False")]
        public void Body_must_be_either_string_or_object(string value, string kind)
        {
            var rawMessage = string.Concat("{\"body\":", value, "}");
            var parser = new SendMessageParser(Encoding.UTF8);

            Action action = () => parser.Parse(rawMessage);

            action.Should().Throw<ApplicationException>().Which.Message.Should().Be(
                $"Message body must be either string or object. Provided value kind {kind} is not supported.");
        }
    }
}